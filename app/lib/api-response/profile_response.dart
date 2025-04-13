class ProfileResponse {
  String? name;
  String? gender;
  String? birthDate;
  String? email;
  String? password;

  ProfileResponse({
    this.name,
    this.gender,
    this.birthDate,
    this.email,
    this.password,
  });

  ProfileResponse.fromJson(Map<String, dynamic> json) {
    name = json['name'];
    gender = json['gender'];
    birthDate = json['birthDate'];
    email = json['email'];
    password = json['password'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = <String, dynamic>{};
    data['name'] = name;
    data['gender'] = gender;
    data['birthDate'] = birthDate;
    data['email'] = email;
    data['password'] = password;
    return data;
  }
}