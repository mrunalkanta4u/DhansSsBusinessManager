//
//  UserDetailsViewController.swift
//  D HansSs Business Manager
//
//  Created by Mrunal Kanta Muduli on 25/10/17.
//  Copyright © 2017 Mrunal Kanta Muduli. All rights reserved.
//

import UIKit

class UserDetailsViewController: UIViewController {

    @IBOutlet weak var userProfilePictureImageView: UIImageView!
    
    @IBOutlet weak var userNameTextfield: UILabel!
    @IBOutlet weak var userRankTextField: UILabel!
    @IBOutlet weak var UserContactNumberTextField: UILabel!
    @IBOutlet weak var userEmailTextField: UILabel!
    
    @IBOutlet weak var userTeamSizeTextField: UILabel!
    @IBOutlet weak var userDRCountTextField: UILabel!
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        UserDefaults.standard.synchronize()
        self.userNameTextfield.text = UserDefaults.standard.string(forKey: "userName")
        self.UserContactNumberTextField.text = UserDefaults.standard.string(forKey: "userContactNumber")
        self.userEmailTextField.text = UserDefaults.standard.string(forKey: "userEmail")
        self.userRankTextField.text = UserDefaults.standard.string(forKey: "userRank")
        let imageData = UserDefaults.standard.value(forKey: "userProfilePicture") as! Data
        self.userProfilePictureImageView.image = UIImage(data: imageData)!
        self.userTeamSizeTextField.text = UserDefaults.standard.string(forKey: "userTeamSize")
        self.userDRCountTextField.text = UserDefaults.standard.string(forKey: "userDRCount")
        // Do any additional setup after loading the view.
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
