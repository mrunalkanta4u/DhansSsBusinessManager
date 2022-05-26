//
//  WelcomeViewController.swift
//  D HansSs Business Manager
//
//  Created by Mrunal Kanta Muduli on 24/10/17.
//  Copyright © 2017 Mrunal Kanta Muduli. All rights reserved.
//

import UIKit

class WelcomeViewController: UIViewController {

    
    @IBOutlet weak var welcomeImageView: UIImageView!
    @IBOutlet weak var welcomeTitleTextField: UILabel!
    @IBOutlet weak var welcomeDetailsMessageTextField: UILabel!

    
    var pageIndex: Int = 0
    var strTitle: String!
    var strDetails: String!
    var strPhotoName: String!

    @IBAction func continueButtonClicked(_ sender: Any) {
       self.dismiss(animated: true, completion:nil)
//        let storyboard = UIStoryboard(name: "Main", bundle: nil)
//        let controller = storyboard.instantiateViewController(withIdentifier: "HomeViewController")
//        self.present(controller, animated: true, completion: nil)
        
    }
    
    
    override func viewDidLoad() {
        super.viewDidLoad()
//        welcomeImageView.image = UIImage(named: strPhotoName)
//        welcomeTitleTextField.text = strTitle
        welcomeDetailsMessageTextField.text = UserDefaults.standard.string(forKey: "userName")
//         Do any additional setup after loading the view.
    }

    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Dispose of any resources that can be recreated.
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


