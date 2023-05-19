import 'package:logger/logger.dart';

class CustomLogger extends Logger {
  final String label;
  CustomLogger(this.label, {printer, output, filter, level})
      : super(printer: printer, output: output, filter: filter, level: level);

  info(String message, {dynamic error, StackTrace? stackTrace}) {
    i('[$label]: $message', error, stackTrace);
  }

  debug(String message, {dynamic error, StackTrace? stackTrace}) {
    d('[$label]: $message', error, stackTrace);
  }

  error(String message, {dynamic error, StackTrace? stackTrace}) {
    e('[$label]: $message', error, stackTrace);
  }

  warning(String message, {dynamic error, StackTrace? stackTrace}) {
    w('[$label]: $message', error, stackTrace);
  }

  verbose(String message, {dynamic error, StackTrace? stackTrace}) {
    v('[$label]: $message', error, stackTrace);
  }
}

class LoggingService {
  static CustomLogger getLogger(String name) {
    return CustomLogger(
      name,
      printer: PrettyPrinter(
        methodCount: 0,
        errorMethodCount: 8,
        lineLength: 120,
        colors: true,
        printEmojis: true,
        printTime: false,
      ),
      output: ConsoleOutput(),
      filter: DevelopmentFilter(),
      level: Level.verbose,
    );
  }
}
