import 'package:deni_hackathon/constants/colors_constants.dart';
import 'package:flutter/material.dart';

import '../../../widgets/deni_style.dart';

class CalendarScreen extends StatelessWidget {
  const CalendarScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: ColorsConstants.baseColor,
      appBar: AppBar(
        backgroundColor: ColorsConstants.baseColor,
        title: const Text('Calendar Screen'),
      ),
      body: Center(child: Text('This is the Calendar Screen')),
    );
  }
}
