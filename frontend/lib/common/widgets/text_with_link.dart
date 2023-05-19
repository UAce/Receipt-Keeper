import 'package:flutter/gestures.dart';
import 'package:flutter/material.dart';
import 'package:receipt_keeper/common/themes.dart';

class TextWithLink extends StatelessWidget {
  final String linkText;
  final VoidCallback onLinkTap;
  final String plainText;
  final FontWeight plainTextWeight;
  final FontWeight linkTextWeight;
  final EdgeInsets padding;

  const TextWithLink(
      {super.key,
      required this.linkText,
      required this.onLinkTap,
      this.plainText = "",
      this.plainTextWeight = FontWeight.w400,
      this.linkTextWeight = FontWeight.bold,
      this.padding = const EdgeInsets.symmetric(vertical: 10)});

  @override
  Widget build(BuildContext context) {
    return Container(
      padding: padding,
      child: RichText(
        text: TextSpan(children: [
          TextSpan(
            text: plainText,
            style: TextStyle(
              color: Colors.black,
              fontSize: 16,
              fontWeight: plainTextWeight,
            ),
          ),
          TextSpan(text: " "),
          TextSpan(
            text: linkText,
            style: TextStyle(
              color: primaryColor,
              fontSize: 16,
              fontWeight: linkTextWeight,
            ),
            recognizer: TapGestureRecognizer()..onTap = onLinkTap,
          )
        ]),
      ),
    );
  }
}
