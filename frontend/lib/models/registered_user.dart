class RegisteredUser {
  final String id;
  final String firstName;
  final String lastName;
  final String email;
  final String fullName;

  RegisteredUser({
    required this.id,
    required this.firstName,
    required this.lastName,
    required this.email,
    required this.fullName,
  });

  factory RegisteredUser.fromJson(Map<String, dynamic> json) {
    return RegisteredUser(
      id: json['id'],
      firstName: json['firstName'],
      lastName: json['lastName'],
      email: json['email'],
      fullName: json['fullName'],
    );
  }
}
