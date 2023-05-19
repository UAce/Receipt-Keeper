import 'dart:convert';

import 'package:http/http.dart';
import 'package:receipt_keeper/common/constants.dart';
import 'package:receipt_keeper/models/registered_user.dart';
import 'package:receipt_keeper/services/firebase_service.dart';
import 'package:receipt_keeper/services/service_locator_service.dart';

import '../logging_service.dart';

class UserApiService {
  final String url = '$baseUrl/User';
  final CustomLogger logger = LoggingService.getLogger('UserApiService');

  Future<RegisteredUser> getByExternalId(String externalId) async {
    logger.info("Retrieving user by external Id [$externalId]");
    logger.debug('$url/$externalId');

    dynamic headers = {
      'Content-Type': 'application/json',
      'authorization':
          'Bearer ${await locator<FirebaseAuthService>().getAuthToken()}'
    };

    Response res = await get(Uri.parse('$url/$externalId'), headers: headers);

    logger.debug('Status code: ${res.statusCode}');
    logger.debug('Body: ${res.body.toString()}');

    // TODO: If statusCode == 401, log out user
    if (res.statusCode == 200) {
      dynamic body = jsonDecode(res.body);
      return RegisteredUser.fromJson(body);
    } else {
      String message = 'Failed to retrieve user by external id [$externalId]';
      logger.error(message, stackTrace: StackTrace.current);
      throw Exception(message);
    }
  }

  // Register user
  Future<RegisteredUser> register(String firstName, String lastName,
      String email, String externalId) async {
    logger.info('Registering new user with external Id [$externalId]');

    String token = await locator<FirebaseAuthService>().getAuthToken();
    Response res = await post(Uri.parse('$url/register'),
        headers: {
          'Content-Type': 'application/json',
          'authorization': 'Bearer $token'
        },
        body: jsonEncode({
          "FirstName": firstName,
          "LastName": lastName,
          "Email": email,
          "ExternalId": externalId,
        }));

    logger.debug('Status code: ${res.statusCode}');
    logger.debug('Body: ${res.body.toString()}');

    if (res.statusCode == 200) {
      dynamic body = jsonDecode(res.body);
      return RegisteredUser.fromJson(body);
    } else {
      String message =
          'Failed to register new user with external ID [$externalId]';
      logger.error(message, stackTrace: StackTrace.current);
      throw Exception(message);
    }
  }
}
