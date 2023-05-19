import 'package:firebase_auth/firebase_auth.dart';
import 'package:receipt_keeper/services/navigation_service.dart';
import 'package:receipt_keeper/services/service_locator_service.dart';

// TODO: Make this a singleton
//  Firebase Authentication SDK wrapper service
class FirebaseAuthService {
  // inject the dependency as a constructor argument
  FirebaseAuthService(this._auth);

  // this property is a dependency
  final FirebaseAuth _auth;

  // Authentication Methods
  Future<User> register(
      {required String email, required String password}) async {
    await _auth.createUserWithEmailAndPassword(
      email: email,
      password: password,
    );

    User? newUser = _auth.currentUser;
    if (newUser == null) {
      throw Exception("Failed to register new user. User is null");
    }
    return newUser;
  }

  Future<User> login({required String email, required String password}) async {
    await _auth.signInWithEmailAndPassword(
      email: email,
      password: password,
    );

    User? loggedInUser = _auth.currentUser;
    if (loggedInUser == null) {
      throw Exception("Failed to log in user. User is null");
    }
    return loggedInUser;
  }

  Future<void> logOut() => _auth.signOut();

  // Listeners
  authStateChanges() => _auth.authStateChanges();

  // Custom Methods
  validateLoginState() {
    final user = _auth.currentUser;
    if (user == null) {
      print('ValidateLoginState: User is currently signed out!');
      locator<NavigationService>().navigateToReplacement("/login");
    } else {
      print('ValidateLoginState: User is currently signed in!');
      locator<NavigationService>().navigateToReplacement("/home");
    }
  }

  listenToLoginState() {
    _auth.authStateChanges().listen((User? user) {
      if (user == null) {
        print('listenToLoginState: User is signed out!');
        locator<NavigationService>().navigateToReplacement("/login");
      } else {
        print('listenToLoginState: User is signed in!');
        locator<NavigationService>().navigateToReplacement("/home");
      }
    });
  }

  User? getCurrentUser() {
    return _auth.currentUser;
  }

  Future<String> getAuthToken() {
    return _auth.currentUser!.getIdToken();
  }
}
