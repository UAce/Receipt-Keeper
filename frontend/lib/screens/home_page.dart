import 'package:flutter/material.dart';
import 'package:receipt_keeper/common/PillButton.dart';
import 'package:receipt_keeper/services/firebase_service.dart';
import 'package:receipt_keeper/services/navigation_service.dart';
import 'package:receipt_keeper/services/service_locator.dart';

class HomePage extends StatelessWidget {
  const HomePage({super.key});

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
                  content: Text("Log out succeeded!"),
                  duration: Duration(seconds: 1)));
            }),
      ),
    );
  }
}
