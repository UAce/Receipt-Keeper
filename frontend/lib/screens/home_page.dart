import 'package:firebase_auth/firebase_auth.dart';
import 'package:flutter/material.dart';
import 'package:receipt_keeper/common/pill_button.dart';
import 'package:receipt_keeper/models/registered_user.dart';
import 'package:receipt_keeper/services/api/user_api_service.dart';
import 'package:receipt_keeper/services/firebase_service.dart';
import 'package:receipt_keeper/services/logging_service.dart';
import 'package:receipt_keeper/services/navigation_service.dart';
import 'package:receipt_keeper/services/service_locator_service.dart';

class HomePage extends StatefulWidget {
  const HomePage({super.key});

  @override
  State<HomePage> createState() => _HomePageState();
}

class _HomePageState extends State<HomePage> {
  String token = "";
  UserApiService userApiService = UserApiService();
  RegisteredUser? user;
  late FirebaseAuthService authService;

  @override
  void initState() {
    super.initState();
    getUser();
  }

  Future<void> getUser() async {
    authService = locator<FirebaseAuthService>();
    // Get auth user
    User? authUser = authService.getCurrentUser();
    if (authUser != null) {
      LoggingService.getLogger('Home')
          .info("Current User: ${authUser.toString()}");
      IdTokenResult value = await authUser.getIdTokenResult();
      setState(() {
        token = value.token!;
      });
      user = await userApiService.getByExternalId(authUser.uid);
    } else {
      locator<NavigationService>().navigateTo('/error');
    }
  }

  @override
  Widget build(BuildContext context) {
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
                  text: user?.email ?? "No user logged in"),
            ),
            const SizedBox(height: 20),
            TextField(
              enabled: false,
              decoration: const InputDecoration(
                border: OutlineInputBorder(),
                labelText: 'Full Name',
              ),
              controller: TextEditingController(
                  text: user?.fullName ?? "No user logged in"),
            ),
            const SizedBox(height: 20),
            TextField(
              enabled: false,
              decoration: const InputDecoration(
                border: OutlineInputBorder(),
                labelText: 'Token',
              ),
              controller: TextEditingController(text: token),
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
            const SizedBox(height: 20),
            PillButton(
                buttonText: "Get user",
                onPressed: () async {
                  await getUser();
                  if (user != null) {
                    ScaffoldMessenger.of(context).showSnackBar(const SnackBar(
                        content: Text("Successfully got user!"),
                        duration: Duration(seconds: 2)));
                  } else {
                    ScaffoldMessenger.of(context).showSnackBar(const SnackBar(
                        content: Text("Failed to get user!"),
                        duration: Duration(seconds: 2)));
                  }
                }),
          ],
        ),
      ),
    );
  }
}
