import 'package:flutter/material.dart';
import 'package:receipt_keeper/common/PillButton.dart';
import 'package:receipt_keeper/common/TextWithLink.dart';
import 'package:receipt_keeper/common/themes.dart';
import 'package:receipt_keeper/services/firebase_service.dart';
import 'package:receipt_keeper/services/navigation_service.dart';
import 'package:receipt_keeper/services/service_locator.dart';

class LoginPage extends StatefulWidget {
  const LoginPage({super.key});

  @override
  State<LoginPage> createState() => _LoginPageState();
}

class _LoginPageState extends State<LoginPage> {
  bool _isProcessing = false;
  bool _isPasswordVisible = false;

  final _authService = locator<FirebaseAuthService>();
  final _registerFormKey = GlobalKey<FormState>();

  final _emailTextController = TextEditingController();
  final _passwordTextController = TextEditingController();

  final _focusEmail = FocusNode();
  final _focusPassword = FocusNode();

  @override
  void initState() {
    super.initState();
    // Start listening to changes.
    // _emailTextController.addListener();
    // _passwordTextController.addListener();
  }

  @override
  void dispose() {
    // Clean up the controller when the widget is removed from the widget tree.
    _emailTextController.dispose();
    _passwordTextController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    double deviceHeight = MediaQuery.of(context).size.height;
    double topSectionHeight = 200.0;

    Future<void> onSubmit() async {
      setState(() => _isProcessing = true);
      print("Submitting login form...");
      Future.delayed(
        const Duration(seconds: 1),
        () => setState(() => _isProcessing = false),
      );

      try {
        await _authService.login(
          email: _emailTextController.text,
          password: _passwordTextController.text,
        );
        ScaffoldMessenger.of(context).showSnackBar(const SnackBar(
            content: Text("Login succeeded!"), duration: Duration(seconds: 1)));
        locator<NavigationService>().navigateToReplacement("/home");
      } catch (e) {
        print(e);
        ScaffoldMessenger.of(context).showSnackBar(const SnackBar(
          content: Text("Invalid username or password"),
        ));
        return;
      }
    }

    return GestureDetector(
        onTap: () {
          _focusEmail.unfocus();
          _focusPassword.unfocus();
        },
        child: Scaffold(
            backgroundColor: lightPrimaryColor,
            body: SingleChildScrollView(
              child: Column(
                children: [
                  SizedBox(
                      height: topSectionHeight,
                      child: Column(
                        mainAxisAlignment: MainAxisAlignment.end,
                        children: [
                          Text("Sign in", style: titleTextStyle),
                          Padding(
                            padding: const EdgeInsets.only(
                              top: 15,
                              bottom: 40,
                            ),
                            child: Text("Welcome to receipt keeper",
                                style: subtitleTextStyle),
                          ),
                        ],
                      )),
                  Container(
                      decoration: const BoxDecoration(
                          color: Colors.white,
                          borderRadius: BorderRadius.only(
                            topLeft: Radius.circular(40),
                            topRight: Radius.circular(40),
                          )),
                      width: double.infinity,
                      height: deviceHeight - topSectionHeight,
                      child: Form(
                        key: _registerFormKey,
                        child: Column(
                          mainAxisAlignment: MainAxisAlignment.spaceAround,
                          children: [
                            Padding(
                              padding: const EdgeInsets.symmetric(
                                  horizontal: 40.0, vertical: 20.0),
                              child: Column(
                                children: [
                                  TextFormField(
                                    focusNode: _focusEmail,
                                    controller: _emailTextController,
                                    validator: (value) {
                                      if (value == null || value.isEmpty) {
                                        return 'Please enter your email';
                                      }
                                      // TODO: add email validation
                                      return null;
                                    },
                                    decoration: const InputDecoration(
                                      border: UnderlineInputBorder(),
                                      labelText: 'Email',
                                    ),
                                  ),
                                  TextFormField(
                                    focusNode: _focusPassword,
                                    controller: _passwordTextController,
                                    obscureText: true,
                                    enableSuggestions: false,
                                    autocorrect: false,
                                    validator: (value) {
                                      if (value == null || value.isEmpty) {
                                        return 'Please enter your password';
                                      }
                                      if (value.length < 8) {
                                        return 'Please enter a valid password';
                                      }
                                      return null;
                                    },
                                    decoration: InputDecoration(
                                      border: UnderlineInputBorder(),
                                      labelText: 'Password',
                                      suffixIcon: IconButton(
                                        icon: Icon(
                                          _isPasswordVisible
                                              ? Icons.visibility
                                              : Icons.visibility_off,
                                        ),
                                        onPressed: () {
                                          setState(() {
                                            _isPasswordVisible =
                                                !_isPasswordVisible;
                                          });
                                        },
                                      ),
                                    ),
                                  ),
                                ],
                              ),
                            ),
                            PillButton(
                                buttonText: "Sign in",
                                isLoading: _isProcessing,
                                onPressed: () async {
                                  if (_registerFormKey.currentState!
                                      .validate()) {
                                    await onSubmit();
                                  }
                                }),
                            TextWithLink(
                                plainText: "Don't have an account?",
                                linkText: "Sign up",
                                onLinkTap: () =>
                                    Navigator.pushNamed(context, '/register'))
                          ],
                        ),
                      ))
                ],
              ),
            )));
  }
}
