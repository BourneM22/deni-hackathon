class SoundboardGetResponse {
  List<Soundboard>? soundboards;

  SoundboardGetResponse({this.soundboards});

  SoundboardGetResponse.fromJson(Map<String, dynamic> json) {
    if (json['data'] != null) {
      soundboards = <Soundboard>[];
      json['data'].forEach((v) {
        soundboards!.add(Soundboard.fromJson(v));
      });
    }
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = <String, dynamic>{};
    if (soundboards != null) {
      data['data'] = soundboards!.map((v) => v.toJson()).toList();
    }
    return data;
  }
}

class Soundboard {
  String? soundId;
  String? filterId;
  String? name;
  String? description;

  Soundboard({
    this.soundId,
    this.filterId,
    this.name,
    this.description,
  });

  Soundboard.fromJson(Map<String, dynamic> json) {
    soundId = json['soundId'] ?? '';
    filterId = json['filterId'] ?? '';
    name = json['name'] ?? '';
    description = json['description'] ?? '';
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = <String, dynamic>{};
    data['soundId'] = soundId;
    data['filterId'] = filterId;
    data['name'] = name;
    data['description'] = description;
    return data;
  }
}