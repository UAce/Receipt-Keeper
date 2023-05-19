import 'dart:async';
import 'dart:io';

import 'package:firebase_auth/firebase_auth.dart';
import 'package:firebase_core/firebase_core.dart';
import 'package:firebase_crashlytics/firebase_crashlytics.dart';
import 'package:flutter/foundation.dart';
import 'package:flutter/material.dart';
import 'package:receipt_keeper/common/constants.dart';
import 'package:receipt_keeper/common/http_overrides.dart';
import 'package:receipt_keeper/common/themes.dart';
import 'package:receipt_keeper/firebase_options.dart';
import 'package:receipt_keeper/screens/error_page.dart';
import 'package:receipt_keeper/screens/home_page.dart';
import 'package:receipt_keeper/screens/register.dart';
import 'package:receipt_keeper/screens/sign_in.dart';
import 'package:receipt_keeper/services/firebase_service.dart';
import 'package:receipt_keeper/services/logging_service.dart';
import 'package:receipt_keeper/services/navigation_service.dart';
import 'package:receipt_keeper/services/service_locator_service.dart';

import 'screens/welcome_page.dart';

void main() async {
  LoggingService.getLogger('Main').info('Starting app in [$environment]');

  // Fix certificate issue in development
  if (environment == 'production') {
    FirebaseCrashlytics.instance.setCrashlyticsCollectionEnabled(true);
  } else {
    HttpOverrides.global = DevelopmentHttpOverrides();
    // FirebaseCrashlytics.instance.setCrashlyticsCollectionEnabled(false);
  }

  // Log errors caught by Zones
  runZonedGuarded<Future<void>>(() async {
    WidgetsFlutterBinding.ensureInitialized();
    await Firebase.initializeApp(
      options: DefaultFirebaseOptions.currentPlatform,
    );

    // Pass all uncaught "fatal" errors from the framework to Crashlytics
    FlutterError.onError = FirebaseCrashlytics.instance.recordFlutterFatalError;

    // Pass all uncaught asynchronous errors that aren't handled by the Flutter framework to Crashlytics
    PlatformDispatcher.instance.onError = (error, stack) {
      FirebaseCrashlytics.instance.recordError(error, stack, fatal: true);
      return true;
    };

    // Register singleton services
    locator.registerLazySingleton<FirebaseAuthService>(
        () => FirebaseAuthService(FirebaseAuth.instance));
    locator.registerLazySingleton<NavigationService>(() => NavigationService());

    final authService = locator<FirebaseAuthService>();
    User? user = authService.getCurrentUser();

    runApp(MyApp(initialRoute: user == null ? '/welcome' : '/home'));
  }, (error, stack) => FirebaseCrashlytics.instance.recordError(error, stack));
}

class MyApp extends StatelessWidget {
  final String initialRoute;

  const MyApp({super.key, required this.initialRoute});

  // This widget is the root of your application.
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Receipt Keeper',
      theme: baseTheme,
      initialRoute: initialRoute,
      routes: {
        '/home': (context) => const HomePage(),
        '/welcome': (context) => const WelcomePage(title: 'Home'),
        '/login': (context) => const LoginPage(),
        '/register': (context) => const RegisterPage(),
        '/error': (context) => const ErrorPage(),
      },
      navigatorKey: locator<NavigationService>().navigatorKey,
    );
  }
}
