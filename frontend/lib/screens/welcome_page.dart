import 'package:flutter/material.dart';
import 'package:flutter_svg/flutter_svg.dart';
import 'package:receipt_keeper/common/themes.dart';
import 'package:receipt_keeper/common/widgets/pill_button.dart';
import 'package:receipt_keeper/common/widgets/text_with_link.dart';

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
            const Spacer(flex: 1),
            Expanded(
                flex: 3,
                child: Align(
                  alignment: Alignment.topCenter,
                  child: Column(children: [
                    const Spacer(),
                    SvgPicture.asset('assets/images/get_started.svg')
                  ]),
                )),
            Expanded(
              flex: 3,
              child: Align(
                alignment: Alignment.bottomCenter,
                child: Column(children: [
                  Container(
                      padding: const EdgeInsets.symmetric(
                          horizontal: 40, vertical: 10),
                      child: Text("Track your receipts anywhere",
                          style: titleTextStyle)),
                  const Spacer(),
                  PillButton(
                      buttonText: "Get Started",
                      onPressed: () {
                        Navigator.pushNamed(context, '/register');
                      }),
                  const SizedBox(height: 10),
                  TextWithLink(
                    linkText: "Sign in",
                    onLinkTap: () {
                      Navigator.pushNamed(context, '/login');
                    },
                  ),
                  const Spacer(),
                ]),
              ),
            )
          ],
        ),
      ),
    );
  }
}
