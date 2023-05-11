import 'package:firebase_auth/firebase_auth.dart';
import 'package:flutter/material.dart';
import 'package:receipt_keeper/common/PillButton.dart';
import 'package:receipt_keeper/services/firebase_service.dart';
import 'package:receipt_keeper/services/navigation_service.dart';
import 'package:receipt_keeper/services/service_locator.dart';

class HomePage extends StatefulWidget {
  const HomePage({super.key});

  @override
  State<HomePage> createState() => _HomePageState();
}

class _HomePageState extends State<HomePage> {
  @override
  initState() {
    super.initState();
    final authService = locator<FirebaseAuthService>();

    User? user = authService.getCurrentUser();
    if (user != null) {
      user.getIdToken().then((value) => print("Auth Token ID: $value"));
    }
  }

  @override
  Widget build(BuildContext context) {
    final authService = locator<FirebaseAuthService>();
    print("Current User: ${authService.getCurrentUser()}");

    return Scaffold(
      appBar: AppBar(
        title: const Text("Home"),
        automaticallyImplyLeading: false,
      ),
      body: Center(
        child: PillButton(
            buttonText: "Log out",
            onPressed: () async {
              await authService.logOut();
              locator<NavigationService>().navigateTo('/login');
              ScaffoldMessenger.of(context).showSnackBar(const SnackBar(
                  content: Text("Successfully logged out!"),
                  duration: Duration(seconds: 2)));
            }),
      ),
    );
  }
}
