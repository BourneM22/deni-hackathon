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
    tokenType = json['token_type'] ?? '';
    accessToken = json['access_token'] ?? '';
    expiresInMinutes = json['expires_in_minutes'] ?? 0;
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = <String, dynamic>{};
    data['token_type'] = tokenType;
    data['access_token'] = accessToken;
    data['expires_in_minutes'] = expiresInMinutes;
    return data;
  }
}