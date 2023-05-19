import 'package:flutter/material.dart';

// TODO: Make this a singleton
class NavigationService {
  // Global navigation key
  final GlobalKey<NavigatorState> navigatorKey = GlobalKey<NavigatorState>();

  Future<dynamic> navigateTo(String routeName) {
    NavigatorState? state = navigatorKey.currentState;
    if (state != null) {
      return state.pushNamed(routeName);
    } else {
      print('NavigationService: NavigatorState is null!');
      return Future.value(null);
    }
  }

  Future<dynamic> navigateToReplacement(String routeName) {
    NavigatorState? state = navigatorKey.currentState;
    if (state != null) {
      return state.pushReplacementNamed(routeName);
    } else {
      print('NavigationService: NavigatorState is null!');
      return Future.value(null);
    }
  }

  void goBack(BuildContext context) {
    NavigatorState? state = navigatorKey.currentState;
    if (state != null) {
      state.maybePop();
    } else {
      ScaffoldMessenger.of(context).showSnackBar(const SnackBar(
          content: Text("Oh no... Please try again later."),
          duration: Duration(seconds: 4)));
      return;
    }
  }
}
