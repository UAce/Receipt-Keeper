import 'package:firebase_auth/firebase_auth.dart';
import 'package:receipt_keeper/services/navigation_service.dart';
import 'package:receipt_keeper/services/service_locator.dart';

// TODO: Make this a singleton
//  Firebase Authentication SDK wrapper service
class FirebaseAuthService {
  // inject the dependency as a constructor argument
  FirebaseAuthService(this._auth);

  // this property is a dependency
  final FirebaseAuth _auth;

  // Authentication Methods
  Future<void> register(
      {required String email, required String password}) async {
    await _auth.createUserWithEmailAndPassword(
      email: email,
      password: password,
    );
    validateLoginState();
  }

  Future<void> login({required String email, required String password}) async {
    await _auth.signInWithEmailAndPassword(
      email: email,
      password: password,
    );
    validateLoginState();
  }

  Future<void> logOut() => _auth.signOut();

  // Listeners
  authStateChanges() => _auth.authStateChanges();

  // Custom Methods
  validateLoginState() {
    final user = getCurrentUser();
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
}
