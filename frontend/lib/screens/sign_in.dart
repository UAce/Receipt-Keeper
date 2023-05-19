import 'package:flutter/material.dart';
import 'package:receipt_keeper/common/pill_button.dart';
import 'package:receipt_keeper/common/text_with_link.dart';
import 'package:receipt_keeper/common/themes.dart';
import 'package:receipt_keeper/services/firebase_service.dart';
import 'package:receipt_keeper/services/service_locator_service.dart';

class LoginPage extends StatefulWidget {
  const LoginPage({super.key});

  @override
  State<LoginPage> createState() => _LoginPageState();
}

class _LoginPageState extends State<LoginPage> {
  bool _isProcessing = false;
  bool _isPasswordVisible = false;
  bool _autoValidateForm = false;
  bool _loginError = false;

  final _authService = locator<FirebaseAuthService>();
  final _loginFormKey = GlobalKey<FormState>();
  final GlobalKey<ScaffoldState> _scaffoldKey = GlobalKey<ScaffoldState>();

  final _emailTextController = TextEditingController();
  final _passwordTextController = TextEditingController();

  final _focusEmail = FocusNode();
  final _focusPassword = FocusNode();

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
      ScaffoldMessenger.of(context).removeCurrentSnackBar();
      if (_loginFormKey.currentState!.validate()) {
        setState(() => _isProcessing = true);

        try {
          await _authService.login(
            email: _emailTextController.text,
            password: _passwordTextController.text,
          );
          ScaffoldMessenger.of(context).showSnackBar(const SnackBar(
              content: Text("Successfully logged in!"),
              backgroundColor: successColor,
              duration: Duration(seconds: 2)));
        } catch (e) {
          if (!_loginError) {
            ScaffoldMessenger.of(context).showSnackBar(const SnackBar(
              content: Text("Invalid username or password"),
              backgroundColor: errorColor,
              duration: Duration(days: 365), // hack to make snackbar persist
            ));
          }
          setState(() => _loginError = true);
          print(e);
        } finally {
          setState(() => _isProcessing = false);
        }
      } else {
        setState(() => _autoValidateForm = true);
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
                        key: _loginFormKey,
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
                                      labelText: 'Email',
                                    ),
                                  ),
                                  TextFormField(
                                    focusNode: _focusPassword,
                                    controller: _passwordTextController,
                                    textInputAction: TextInputAction.done,
                                    obscureText: true,
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
                                onPressed: () async => await onSubmit()),
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
