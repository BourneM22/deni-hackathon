import 'package:flutter/material.dart';

import '../../../constants/colors_constants.dart';
import '../../../widgets/deni_style.dart';

class SoundboardScreen extends StatelessWidget {
  const SoundboardScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: ColorsConstants.baseColor,
      appBar: AppBar(
        backgroundColor: ColorsConstants.baseColor,
        title: const Text('Soundboard Screen'),
      ),
      body: Center(child: Text('This is the Soundboard Screen')),
    );
  }
}
