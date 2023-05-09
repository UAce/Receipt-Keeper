import 'package:flutter/material.dart';
import 'package:receipt_keeper/common/PillButton.dart';
import 'package:receipt_keeper/common/TextWithLink.dart';
import 'package:receipt_keeper/common/themes.dart';

class RegisterPage extends StatefulWidget {
  const RegisterPage({super.key});

  @override
  State<RegisterPage> createState() => _RegisterPageState();
}

class _RegisterPageState extends State<RegisterPage> {
  final _registerFormKey = GlobalKey<FormState>();

  final _firstNameTextController = TextEditingController();
  final _lastNameTextController = TextEditingController();
  final _emailTextController = TextEditingController();
  final _passwordTextController = TextEditingController();

  final _focusFirstName = FocusNode();
  final _focusLastName = FocusNode();
  final _focusEmail = FocusNode();
  final _focusPassword = FocusNode();

  bool _isProcessing = false;
  bool _isPasswordVisible = false;

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
          _focusFirstName.unfocus();
          _focusLastName.unfocus();
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
                          Text("Sign up", style: titleTextStyle),
                          Padding(
                            padding: const EdgeInsets.only(
                              top: 15,
                              bottom: 40,
                            ),
                            child: Text("Create an account to get started",
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
                                    focusNode: _focusFirstName,
                                    validator: (value) {
                                      if (value == null || value.isEmpty) {
                                        return 'Please enter your first name';
                                      }
                                      if (value.length < 2) {
                                        return 'Must be at least 2 characters';
                                      }
                                      return null;
                                    },
                                    decoration: const InputDecoration(
                                      border: UnderlineInputBorder(),
                                      labelText: 'First name',
                                    ),
                                  ),
                                  TextFormField(
                                    focusNode: _focusLastName,
                                    validator: (value) {
                                      if (value == null || value.isEmpty) {
                                        return 'Please enter your last name';
                                      }
                                      if (value.length < 2) {
                                        return 'Must be at least 2 characters';
                                      }
                                      return null;
                                    },
                                    decoration: const InputDecoration(
                                      border: UnderlineInputBorder(),
                                      labelText: 'Last name',
                                    ),
                                  ),
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
                                    obscureText: !_isPasswordVisible,
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
                                    decoration: InputDecoration(
                                      border: const UnderlineInputBorder(),
                                      labelText: 'Password',
                                      helperText:
                                          'Must be at least 8 characters',
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
                                buttonText: "Sign up",
                                isLoading: _isProcessing,
                                onPressed: () async {
                                  if (_registerFormKey.currentState!
                                      .validate()) {
                                    await onSubmit();
                                  }
                                }),
                            TextWithLink(
                                plainText: "Already have an account?",
                                linkText: "Sign in",
                                onLinkTap: () =>
                                    Navigator.pushNamed(context, '/signIn'))
                          ],
                        ),
                      ))
                ],
              ),
            )));
  }
}
