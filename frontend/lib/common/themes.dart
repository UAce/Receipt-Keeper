import 'package:flutter/material.dart';

const primaryColor = Color(0xFF7B55E0);
const lightPrimaryColor = Color(0xFFCEBBFA);

ThemeData baseTheme = ThemeData(
    brightness: Brightness.light,
    primaryColor: primaryColor,
    appBarTheme: const AppBarTheme(backgroundColor: primaryColor));

TextStyle titleTextStyle = const TextStyle(
  fontSize: 26,
  fontWeight: FontWeight.w800,
);

TextStyle subtitleTextStyle = const TextStyle(
  fontSize: 16,
  fontWeight: FontWeight.w400,
);
