import 'package:flutter/material.dart';
import 'package:receipt_keeper/common/PillButton.dart';
import 'package:receipt_keeper/common/TextWithLink.dart';
import 'package:receipt_keeper/common/themes.dart';
import 'package:receipt_keeper/services/firebase_service.dart';
import 'package:receipt_keeper/services/service_locator.dart';

class RegisterPage extends StatefulWidget {
  const RegisterPage({super.key});

  @override
  State<RegisterPage> createState() => _RegisterPageState();
}

class _RegisterPageState extends State<RegisterPage> {
  bool _isProcessing = false;
  bool _isPasswordVisible = false;
  bool _autoValidateForm = false;

  final _authService = locator<FirebaseAuthService>();
  final _registerFormKey = GlobalKey<FormState>();

  // Field Focus Nodes
  final _focusFirstName = FocusNode();
  final _focusLastName = FocusNode();
  final _focusEmail = FocusNode();
  final _focusPassword = FocusNode();

  // Field Text Controllers
  final _firstNameTextController = TextEditingController();
  final _lastNameTextController = TextEditingController();
  final _emailTextController = TextEditingController();
  final _passwordTextController = TextEditingController();

  @override
  void dispose() {
    // Clean up the controller when the widget is removed from the widget tree.
    _firstNameTextController.dispose();
    _lastNameTextController.dispose();
    _emailTextController.dispose();
    _passwordTextController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    double deviceHeight = MediaQuery.of(context).size.height;
    double topSectionHeight = 200.0;

    Future<void> onSubmit() async {
      ScaffoldMessenger.of(context).removeCurrentSnackBar();
      if (_registerFormKey.currentState!.validate()) {
        setState(() => _isProcessing = true);

        try {
          await _authService.register(
            email: _emailTextController.text,
            password: _passwordTextController.text,
          );
          ScaffoldMessenger.of(context).showSnackBar(const SnackBar(
              content: Text("Successfully created account!"),
              backgroundColor: successColor,
              duration: Duration(seconds: 2)));
        } catch (e) {
          print(e);
          ScaffoldMessenger.of(context).showSnackBar(const SnackBar(
            content: Text("Something went wrong. Please try again."),
            backgroundColor: errorColor,
            duration: Duration(days: 365), // hack to make snackbar persist
          ));
          return;
        } finally {
          setState(() => _isProcessing = false);
        }
      } else {
        setState(() => _autoValidateForm = true);
      }
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
                        autovalidateMode: _autoValidateForm
                            ? AutovalidateMode.onUserInteraction
                            : AutovalidateMode.disabled,
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
                                    controller: _firstNameTextController,
                                    textInputAction: TextInputAction.next,
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
                                    controller: _lastNameTextController,
                                    textInputAction: TextInputAction.next,
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
                                    controller: _emailTextController,
                                    textInputAction: TextInputAction.next,
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
                                    textInputAction: TextInputAction.done,
                                    obscureText: !_isPasswordVisible,
                                    enableSuggestions: false,
                                    autocorrect: false,
                                    // onFieldSubmitted expects a synchronous function
                                    // so I cannot call onSubmit() with await
                                    onFieldSubmitted: (value) => onSubmit(),
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
                                onPressed: () async => await onSubmit()),
                            TextWithLink(
                                plainText: "Already have an account?",
                                linkText: "Sign in",
                                onLinkTap: () =>
                                    Navigator.pushNamed(context, '/login'))
                          ],
                        ),
                      ))
                ],
              ),
            )));
  }
}
