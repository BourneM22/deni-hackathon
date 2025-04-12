import 'package:flutter/material.dart';

import '../../../constants/colors_constants.dart';
import '../../../widgets/deni_style.dart';

class NotesScreen extends StatelessWidget {
  const NotesScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: ColorsConstants.baseColor,
      appBar: AppBar(
        backgroundColor: ColorsConstants.baseColor,
        title: const Text('Notes Screen'),
      ),
      body: Center(child: Text('This is the Notes Screen')),
    );
  }
}
