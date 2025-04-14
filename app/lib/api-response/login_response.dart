class LoginResponse {
  String? tokenType;
  String? accessToken;
  int? expiresInMinutes;

  LoginResponse({
    this.tokenType,
    this.accessToken,
    this.expiresInMinutes,
  });

  LoginResponse.fromJson(Map<String, dynamic> json) {
    tokenType = json['tokenType'] ?? '';
    accessToken = json['accessToken'] ?? '';
    expiresInMinutes = json['expiresInMinutes'] ?? 0;
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = <String, dynamic>{};
    data['tokenType'] = tokenType;
    data['accessToken'] = accessToken;
    data['expiresInMinutes'] = expiresInMinutes;
    return data;
  }
}