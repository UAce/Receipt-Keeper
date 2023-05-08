import 'package:flutter/material.dart';
import 'package:flutter_svg/flutter_svg.dart';
import 'package:receipt_keeper/common/themes.dart';

class WelcomePage extends StatelessWidget {
  final String title;

  const WelcomePage({super.key, required this.title});

  @override
  Widget build(BuildContext context) {
    // This method is rerun every time setState is called, for instance as done
    // by the _incrementCounter method above.
    //
    // The Flutter framework has been optimized to make rerunning build methods
    // fast, so that you can just rebuild anything that needs updating rather
    // than having to individually change instances of widgets.
    return Scaffold(
      // appBar: AppBar(
      //   // Here we take the value from the WelcomePage object that was created by
      //   // the App.build method, and use it to set our appbar title.
      //   title: Text(title),
      // ),
      body: Center(
        // Center is a layout widget. It takes a single child and positions it
        // in the middle of the parent.
        child: Column(
          // Column is also a layout widget. It takes a list of children and
          // arranges them vertically. By default, it sizes itself to fit its
          // children horizontally, and tries to be as tall as its parent.
          //
          // Invoke "debug painting" (press "p" in the console, choose the
          // "Toggle Debug Paint" action from the Flutter Inspector in Android
          // Studio, or the "Toggle Debug Paint" command in Visual Studio Code)
          // to see the wireframe for each widget.
          //
          // Column has various properties to control how it sizes itself and
          // how it positions its children. Here we use mainAxisAlignment to
          // center the children vertically; the main axis here is the vertical
          // axis because Columns are vertical (the cross axis would be
          // horizontal).
          mainAxisAlignment: MainAxisAlignment.center,
          children: <Widget>[
            Expanded(
                child: Align(
              alignment: Alignment.topCenter,
              child: Column(children: [
                Spacer(),
                SvgPicture.asset('assets/images/get_started.svg')
              ]),
            )),
            Expanded(
              child: Align(
                alignment: Alignment.bottomCenter,
                child: Column(children: [
                  Container(
                      padding:
                          EdgeInsets.symmetric(horizontal: 40, vertical: 10),
                      child: const Text(
                        "Track your receipts anywhere",
                        style: TextStyle(
                            fontWeight: FontWeight.bold, fontSize: 26),
                      )),
                  Spacer(),
                  FilledButton(
                    onPressed: () {
                      Navigator.pushNamed(context, '/register');
                    },
                    style: ButtonStyle(
                      backgroundColor: MaterialStateProperty.all<Color>(
                          const Color(0xFF7B55E0)),
                      padding: MaterialStateProperty.all<EdgeInsets>(
                          EdgeInsets.symmetric(horizontal: 80, vertical: 20)),
                    ),
                    child: const Text("Get Started",
                        style: TextStyle(
                            fontWeight: FontWeight.w800, fontSize: 16)),
                  ),
                  TextButton(
                      onPressed: () {
                        Navigator.pushNamed(context, '/signIn');
                      },
                      child: const Text("Sign in",
                          style: TextStyle(
                              color: primaryColor,
                              fontWeight: FontWeight.w800,
                              fontSize: 16))),
                  Spacer(),
                ]),
              ),
            )
          ],
        ),
      ),
    );
  }
}
