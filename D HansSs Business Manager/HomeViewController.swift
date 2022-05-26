//
//  HomeViewController.swift
//  D HansSs Business Manager
//
//  Created by Mrunal Kanta Muduli on 24/10/17.
//  Copyright © 2017 Mrunal Kanta Muduli. All rights reserved.
//

import UIKit

class HomeViewController: UITabBarController {

    override func viewDidLoad() {
        super.viewDidLoad()

        // Do any additional setup after loading the view.
    }

    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Dispose of any resources that can be recreated.
    }
    
//    var freshLaunch = true

    override func viewDidAppear(_ animated: Bool) {
        
        
        let launchedBefore = UserDefaults.standard.bool(forKey: "isFirstLaunch")
        if launchedBefore  {
            print("Not first launch.")
            let isUserLoggedIn = UserDefaults.standard.bool(forKey: "isUserLoggedIn")
            if(!isUserLoggedIn){
                self.performSegue(withIdentifier: "accountLoginView", sender: self)
            }
            else{
                let isFirstLogIn = UserDefaults.standard.bool(forKey: "isFirstLogIn")
                if(isFirstLogIn){
                    self.selectedIndex = 0
                    UserDefaults.standard.set(false,forKey:"isFirstLogIn");
                    UserDefaults.standard.synchronize();
                }
            }
        } else {
            print("First launch, setting UserDefault.")

            let storyboard = UIStoryboard(name: "Main", bundle: nil)
            let controller = storyboard.instantiateViewController(withIdentifier: "WelcomeViewController")
            self.present(controller, animated: true, completion: nil)
            
            
            UserDefaults.standard.set(true, forKey: "isFirstLaunch")
            return
        }
        
        
        
        

    }

    /*
    // MARK: - Navigation

    // In a storyboard-based application, you will often want to do a little preparation before navigation
    override func prepare(for segue: UIStoryboardSegue, sender: Any?) {
        // Get the new view controller using segue.destinationViewController.
        // Pass the selected object to the new view controller.
    }
    */

}
