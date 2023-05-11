import 'package:firebase_auth/firebase_auth.dart';
import 'package:firebase_core/firebase_core.dart';
import 'package:flutter/material.dart';
import 'package:receipt_keeper/common/themes.dart';
import 'package:receipt_keeper/firebase_options.dart';
import 'package:receipt_keeper/screens/home_page.dart';
import 'package:receipt_keeper/screens/register.dart';
import 'package:receipt_keeper/screens/sign_in.dart';
import 'package:receipt_keeper/services/firebase_service.dart';
import 'package:receipt_keeper/services/navigation_service.dart';
import 'package:receipt_keeper/services/service_locator.dart';

import 'screens/welcome_page.dart';

void main() async {
  WidgetsFlutterBinding.ensureInitialized();
  await Firebase.initializeApp(
    options: DefaultFirebaseOptions.currentPlatform,
  );

  // Register singleton services
  locator.registerLazySingleton(() => NavigationService());
  locator
      .registerLazySingleton(() => FirebaseAuthService(FirebaseAuth.instance));

  final authService = locator<FirebaseAuthService>();
  User? user = authService.getCurrentUser();

  runApp(MyApp(initialRoute: user == null ? '/welcome' : '/home'));
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
      },
      navigatorKey: locator<NavigationService>().navigatorKey,
    );
  }
}
