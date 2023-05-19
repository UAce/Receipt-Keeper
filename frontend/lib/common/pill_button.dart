import 'package:flutter/material.dart';

class PillButton extends StatelessWidget {
  final String buttonText;
  final VoidCallback onPressed;
  final bool isLoading;

  const PillButton(
      {super.key,
      required this.buttonText,
      required this.onPressed,
      this.isLoading = false});

  @override
  Widget build(BuildContext context) {
    return SizedBox(
      width: 200,
      height: 60,
      child: FilledButton(
          onPressed: isLoading ? null : onPressed,
          style: ButtonStyle(
            backgroundColor:
                MaterialStateProperty.all<Color>(const Color(0xFF7B55E0)),
            padding: MaterialStateProperty.all<EdgeInsets>(
                const EdgeInsets.symmetric(horizontal: 20, vertical: 20)),
          ),
          child: isLoading
              ? const SizedBox(
                  height: 18,
                  width: 18,
                  child: CircularProgressIndicator(
                    color: Colors.white,
                    strokeWidth: 3,
                  ),
                )
              : Text(buttonText,
                  style: const TextStyle(
                      fontWeight: FontWeight.w800, fontSize: 16))),
    );
  }
}
