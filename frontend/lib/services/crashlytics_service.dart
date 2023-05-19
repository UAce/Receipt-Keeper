import 'package:firebase_crashlytics/firebase_crashlytics.dart';

class CrashlyticsService {
  static recordError(
      {required String reason,
      dynamic error,
      required StackTrace stackTrace,
      bool fatal = false}) {
    FirebaseCrashlytics.instance
        .recordError(error, stackTrace, reason: reason, fatal: fatal);
  }

  static log(String message) {
    FirebaseCrashlytics.instance.log(message);
  }
}
