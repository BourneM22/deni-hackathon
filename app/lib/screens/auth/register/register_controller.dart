import 'package:deni_hackathon/routes/route_name.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';



class RegisterController extends GetxController {

  final nameTFController = TextEditingController();
  final genderTFController = TextEditingController();
  final birthDateTFController = TextEditingController();
  final emailTFController = TextEditingController();
  final passwordTFController = TextEditingController();
  var birthDate = Rxn<DateTime>();

  @override
  void onInit() {
    super.onInit();
  }

  @override
  void dispose() {
    super.dispose();
  }


  Future<void> register() async {
    Get.toNamed(MainRoute.main);
  }
}