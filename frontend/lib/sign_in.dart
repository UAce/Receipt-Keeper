import 'package:flutter/material.dart';
import 'package:receipt_keeper/common/PillButton.dart';
import 'package:receipt_keeper/common/TextWithLink.dart';
import 'package:receipt_keeper/common/themes.dart';

class SignInPage extends StatefulWidget {
  const SignInPage({super.key});

  @override
  State<SignInPage> createState() => _SignInPageState();
}

class _SignInPageState extends State<SignInPage> {
  final _registerFormKey = GlobalKey<FormState>();

  final _emailTextController = TextEditingController();
  final _passwordTextController = TextEditingController();

  final _focusEmail = FocusNode();
  final _focusPassword = FocusNode();

  bool _isProcessing = false;

  @override
  Widget build(BuildContext context) {
    double deviceHeight = MediaQuery.of(context).size.height;
    double topSectionHeight = 200.0;

    Future<void> onSubmit() async {
      setState(() => _isProcessing = true);
      Future.delayed(
        const Duration(seconds: 2),
        () => setState(() => _isProcessing = false),
      );
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
                                    obscureText: true,
                                    enableSuggestions: false,
                                    autocorrect: false,
                                    focusNode: _focusPassword,
                                    validator: (value) {
                                      if (value == null || value.isEmpty) {
                                        return 'Please enter your password';
                                      }
                                      if (value.length < 8) {
                                        return 'Please enter a valid password';
                                      }
                                      return null;
                                    },
                                    decoration: const InputDecoration(
                                      border: UnderlineInputBorder(),
                                      labelText: 'Password',
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
