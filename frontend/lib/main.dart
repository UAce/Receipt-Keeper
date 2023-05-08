import 'package:flutter/material.dart';
import 'package:receipt_keeper/common/themes.dart';

import 'welcome_page.dart';

void main() async {
  WidgetsFlutterBinding.ensureInitialized();
  runApp(MyApp(initialRoute: '/welcome'));
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
          '/welcome': (context) => const WelcomePage(title: 'Home'),
          '/signIn': (context) => const Text("TODO: Create Sign In Page"),
          '/register': (context) => const Text("TODO: Create Register Page"),
        });
  }
}
