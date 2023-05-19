import 'package:flutter/material.dart';
import 'package:receipt_keeper/common/pill_button.dart';
import 'package:receipt_keeper/common/themes.dart';
import 'package:receipt_keeper/services/navigation_service.dart';
import 'package:receipt_keeper/services/service_locator_service.dart';

class ErrorPage extends StatefulWidget {
  const ErrorPage({super.key});

  @override
  State<ErrorPage> createState() => _ErrorPageState();
}

class _ErrorPageState extends State<ErrorPage> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Center(
        child: Column(
          children: [
            const Expanded(child: SizedBox()),
            Text("Oops something went wrong!", style: subtitleTextStyle),
            const SizedBox(height: 20),
            PillButton(
                buttonText: "Retry",
                onPressed: () async {
                  locator<NavigationService>().goBack(context);
                }),
            const Expanded(child: SizedBox()),
          ],
        ),
      ),
    );
  }
}
