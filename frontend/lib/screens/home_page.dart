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
  String token = "";

  @override
  initState() {
    super.initState();
    final authService = locator<FirebaseAuthService>();

    User? user = authService.getCurrentUser();
    if (user != null) {
      user.getIdTokenResult().then((value) {
        print("Token Result: $value");
        setState(() {
          token = value.token!;
        });
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    final authService = locator<FirebaseAuthService>();
    final user = authService.getCurrentUser();
    print("Current User: ${authService.getCurrentUser()}");

    return Scaffold(
      appBar: AppBar(
        title: const Text("Home"),
        automaticallyImplyLeading: false,
      ),
      body: Center(
        child: Column(
          children: [
            const SizedBox(height: 100),
            TextField(
              enabled: false,
              decoration: const InputDecoration(
                border: OutlineInputBorder(),
                labelText: 'Email',
              ),
              controller: TextEditingController(
                  text: user != null ? user.email : "No user logged in"),
            ),
            SizedBox(height: 20),
            TextField(
              enabled: false,
              decoration: const InputDecoration(
                border: OutlineInputBorder(),
                labelText: 'Token',
              ),
              controller:
                  TextEditingController(text: token ?? "No user logged in"),
            ),
            const SizedBox(height: 20),
            PillButton(
                buttonText: "Log out",
                onPressed: () async {
                  await authService.logOut();
                  locator<NavigationService>().navigateTo('/login');
                  ScaffoldMessenger.of(context).showSnackBar(const SnackBar(
                      content: Text("Successfully logged out!"),
                      duration: Duration(seconds: 2)));
                }),
          ],
        ),
      ),
    );
  }
}
