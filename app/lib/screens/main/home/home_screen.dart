import 'package:deni_hackathon/constants/colors_constants.dart';
import 'package:deni_hackathon/screens/main/home/home_controller.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';

import '../../../widgets/deni_style.dart';

class HomeScreen extends StatelessWidget {
  const HomeScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return GetBuilder<HomeController>(
      init: HomeController(),
      builder: (HomeController controller) {
        return Scaffold(
          backgroundColor: ColorsConstants.baseColor,
          appBar: AppBar(
            automaticallyImplyLeading: false,
            backgroundColor: ColorsConstants.baseColor,
            elevation: 0,
            centerTitle: true,
            title: Text(
              'Listening',
              style: deniStyle(color: Colors.red, fontWeight: FontWeight.bold),
            ),
            actions: const [
              Padding(
                padding: EdgeInsets.only(right: 16),
                child: Icon(Icons.delete_outline, color: Colors.black),
              ),
            ],
            leading: const Padding(
              padding: EdgeInsets.only(left: 16),
              child: Icon(Icons.person_outline, color: Colors.black),
            ),
          ),
          body: RefreshIndicator(
            onRefresh: () async {
              controller.onInit();

            },
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                SizedBox(
                  height: MediaQuery.of(context).size.height * 0.4,
                  child: Padding(
                    padding: EdgeInsets.all(16.0),
                    child: Text(
                      "Hello I'm Bourne!\nWhat's your name?",
                      style: deniStyle(fontSize: 16),
                    ),
                  ),
                ),
                Expanded(
                  child: Container(
                    padding: const EdgeInsets.only(top: 8),
                    decoration: BoxDecoration(
                      color: ColorsConstants.whiteColor,
                      borderRadius: const BorderRadius.only(
                        topLeft: Radius.circular(20),
                        topRight: Radius.circular(20),
                      ),
                    ),
                    child: Column(
                      children: [
                        Padding(
                          padding: const EdgeInsets.symmetric(horizontal: 16.0),
                          child: Row(
                            mainAxisAlignment: MainAxisAlignment.start,
                            children: [
                              Container(
                                padding: const EdgeInsets.symmetric(
                                  vertical: 4,
                                  horizontal: 12,
                                ),
                                decoration: BoxDecoration(
                                  color: ColorsConstants.darkerWhiteColor,
                                  borderRadius: BorderRadius.circular(20),
                                ),
                                child: const Text('All'),
                              ),
                            ],
                          ),
                        ),
                        const SizedBox(height: 8),
                        Expanded(
                          child: controller.isLoading
                              ? const Center(
                                  child: CircularProgressIndicator(),
                                )
                              :
                           GridView.builder(
                            padding: const EdgeInsets.symmetric(horizontal: 16),
                            gridDelegate:
                                const SliverGridDelegateWithFixedCrossAxisCount(
                                  crossAxisCount: 2,
                                  crossAxisSpacing: 8,
                                  mainAxisSpacing: 8,
                                  childAspectRatio: 2,
                                ),
                            itemCount: controller.soundboardList.length,
                            itemBuilder: (context, index) {
                              final soundboard = controller.soundboardList[index];
                              return Container(
                                padding: const EdgeInsets.all(8),
                                decoration: BoxDecoration(
                                  color: ColorsConstants.darkerWhiteColor,
                                  borderRadius: BorderRadius.circular(10),
                                ),
                                child: Column(
                                  crossAxisAlignment: CrossAxisAlignment.start,
                                  children: [
                                    Text(
                                      soundboard.name!,
                                      style: deniStyle(
                                        fontWeight: FontWeight.bold,
                                      ),
                                    ),
                                    const SizedBox(height: 4),
                                    Text(
                                      soundboard.description!,
                                      style: deniStyle(fontSize: 12),
                                      maxLines: 2,
                                      overflow: TextOverflow.ellipsis,
                                    ),
                                  ],
                                ),
                              );
                            },
                          ),
                        ),
                      ],
                    ),
                  ),
                ),
              ],
            ),
          ),
        );
      }
    );
  }
}
