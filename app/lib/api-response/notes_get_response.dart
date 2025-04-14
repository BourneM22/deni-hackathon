class Notes {
  String? noteId;
  String? tagId;
  String? title;
  String? content;
  DateTime? date;

  Notes({
    this.noteId,
    this.tagId,
    this.title,
    this.content,
    this.date,
  });

  Notes.fromJson(Map<String, dynamic> json) {
    noteId = json['noteId'] ?? '';
    tagId = json['tagId'];
    title = json['title'] ?? '';
    content = json['content'] ?? '';
    date = json['date'] != null ? DateTime.parse(json['date']) : null;
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = <String, dynamic>{};
    data['noteId'] = noteId;
    data['tagId'] = tagId;
    data['title'] = title;
    data['content'] = content;
    data['date'] = date?.toIso8601String();
    return data;
  }
}

class NotesGetResponse {
  List<Notes>? notes;

  NotesGetResponse({this.notes});

  NotesGetResponse.fromJson(Map<String, dynamic> json) {
    if (json['data'] != null) {
      notes = <Notes>[];
      json['data'].forEach((v) {
        notes!.add(Notes.fromJson(v));
      });
    }
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = <String, dynamic>{};
    if (notes != null) {
      data['data'] = notes!.map((v) => v.toJson()).toList();
    }
    return data;
  }
}