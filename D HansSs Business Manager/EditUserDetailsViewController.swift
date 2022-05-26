//
//  EditUserDetailsViewController.swift
//  D HansSs Business Manager
//
//  Created by Mrunal Kanta Muduli on 25/10/17.
//  Copyright © 2017 Mrunal Kanta Muduli. All rights reserved.
//

import UIKit

class EditUserDetailsViewController: UITableViewController , UIImagePickerControllerDelegate, UINavigationControllerDelegate  {
    
    override func viewDidLoad() {
        super.viewDidLoad()
        UserDefaults.standard.synchronize()
        self.NameDetailsTextField.text = UserDefaults.standard.string(forKey: "userName")
        self.ContactNumberDetailsTextField.text = UserDefaults.standard.string(forKey: "userContactNumber")
        self.EmailIDDetailsTextField.text = UserDefaults.standard.string(forKey: "userEmail")
        self.RankDetailsTextField.text = UserDefaults.standard.string(forKey: "userRank")
        let imageData = UserDefaults.standard.value(forKey: "userProfilePicture") as! Data
        self.profileImageView.image = UIImage(data: imageData)!
        self.TeamSizeDetailsTextField.text = UserDefaults.standard.string(forKey: "userTeamSize")
        self.DRCountDetailsTextField.text = UserDefaults.standard.string(forKey: "userDRCount")
        // Do any additional setup after loading the view.
    }
    
    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Dispose of any resources that can be recreated.
    }
    
    @IBOutlet weak var profileImageView: UIImageView!
    
    @IBOutlet weak var NameDetailsTextField: UILabel!
    @IBOutlet weak var RankDetailsTextField: UILabel!
    @IBOutlet weak var ContactNumberDetailsTextField: UILabel!
    @IBOutlet weak var EmailIDDetailsTextField: UILabel!
    @IBOutlet weak var TeamSizeDetailsTextField: UILabel!
    @IBOutlet weak var DRCountDetailsTextField: UILabel!
    
    func presentAlert(title: String, message: String, key: String){
        let alertController = UIAlertController(title: title, message: message, preferredStyle: .alert)
        
        let confirmAction = UIAlertAction(title: "Confirm", style: .default) { (_) in
            if let field = alertController.textFields?[0] {
                
                switch key{
                case "userName":
                    self.NameDetailsTextField.text = field.text
                case "userRank":
                    self.RankDetailsTextField.text = field.text
                case "userContactNumber":
                    self.ContactNumberDetailsTextField.text = field.text
                case "userTeamSize":
                    self.TeamSizeDetailsTextField.text = field.text
                case "userDRCount":
                    self.DRCountDetailsTextField.text = field.text
                default:
                    print("default case")
                    
                }

                
                // store your data
                
                UserDefaults.standard.set(field.text, forKey: key)
                UserDefaults.standard.synchronize()
            } else {
                // user did not fill field
            }
        }
        
        let cancelAction = UIAlertAction(title: "Cancel", style: .cancel) { (_) in }
        
        alertController.addTextField { (textField) in
            textField.placeholder = title
        }
        
        alertController.addAction(confirmAction)
        alertController.addAction(cancelAction)
        
        if (key == "userContactNumber")
        {
            alertController.textFields?[0].keyboardType = UIKeyboardType.phonePad
        }
        else if (key == "userTeamSize" || key ==  "userDRCount")
        {
            alertController.textFields?[0].keyboardType = UIKeyboardType.numberPad
        }
        else{
            alertController.textFields?[0].keyboardType = UIKeyboardType.default
        }
        self.present(alertController, animated: true, completion: nil)
    }
    func presentError(title: String, message: String){
        let alert = UIAlertController(title: title, message: message, preferredStyle: UIAlertControllerStyle.alert)
        alert.addAction(UIAlertAction(title: "OK", style: UIAlertActionStyle.default, handler: nil))
        self.present(alert, animated: true, completion: nil)
    }
    
    func updateField(rowIndex: Int) {
        
        var title = ""
        var message = ""
        var key = ""
        switch rowIndex {
        case 1:
            title = "Name"
            message = "Please input your Name:"
            key = "userName"
            
        case 2:
            title = "Rank"
            message = "Please input your Rank:"
            key = "userRank"
            
        case 3:
            title = "Contact Number"
            message = "Please input your Contact Number:"
            key = "userContactNumber"
            
        case 4:
            title = "Sorry!!!"
            message = "Email ID change is Not Alllowed"
            key = "userEmail"
            
        case 5:
            title = "Team Size"
            message = "Please input your Team Size:"
            key = "userTeamSize"
            
        case 6:
            title = "DR Count"
            message = "Please input your DR Count:"
            key = "userDRCount"
            
        default:
            print("Invalid Row Selected")
        }
        if rowIndex == 4
        {
            presentError(title: title,message: message)
        }
        else{
            presentAlert(title: title, message: message, key: key)
        }
        
    }
    
    
    override func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
        print("You selected cell #\(indexPath.row)!")
        let rowIndex = indexPath.row
        updateField(rowIndex: rowIndex )
    }
    
    @IBAction func changeImageTapped(_ sender: Any) {
        let pickerController = UIImagePickerController()
        pickerController.delegate = self
        pickerController.allowsEditing = true
        
        let alertController = UIAlertController(title: "Add a Picture", message: "Choose From", preferredStyle: .actionSheet)
        
        let cameraAction = UIAlertAction(title: "Camera", style: .default) { (action) in
            if(UIImagePickerController.isSourceTypeAvailable(UIImagePickerControllerSourceType.camera))
            {
                pickerController.sourceType = .camera
                self.present(pickerController, animated: true, completion: nil)
            }
            else
            {
                let alert = UIAlertController(title: "Oops!!!", message: "Camera Not found", preferredStyle: .alert)
                let ok = UIAlertAction(title: "Ok", style: .default, handler: nil)
                alert.addAction(ok)
                self.present(alert, animated: true, completion: nil)
            }
        }
        let photosLibraryAction = UIAlertAction(title: "Photos Library", style: .default) { (action) in
            pickerController.sourceType = .photoLibrary
            self.present(pickerController, animated: true, completion: nil)
            
        }
        
        let savedPhotosAction = UIAlertAction(title: "Saved Photos Album", style: .default) { (action) in
            pickerController.sourceType = .savedPhotosAlbum
            self.present(pickerController, animated: true, completion: nil)
            
        }
        
        let cancelAction = UIAlertAction(title: "Cancel", style: .destructive, handler: nil)
        
        alertController.addAction(cameraAction)
        alertController.addAction(photosLibraryAction)
        alertController.addAction(savedPhotosAction)
        alertController.addAction(cancelAction)
        
        present(alertController, animated: true, completion: nil)
        
    }
    
    func imagePickerController(_ picker: UIImagePickerController, didFinishPickingMediaWithInfo info: [String : Any])
    {
        if let image = info[UIImagePickerControllerEditedImage] as? UIImage
        {
            profileImageView.image = image
            UserDefaults.standard.set(UIImageJPEGRepresentation(image, 100), forKey: "userProfilePicture")
        }
        else
        {
            //Error message
        }
        
        self.dismiss(animated: true, completion: nil)
    }
    
    /*
     // MARK: - Navigation
     
     // In a storyboard-based application, you will often want to do a little preparation before navigation
     override func prepare(for segue: UIStoryboardSegue, sender: Any?) {
     // Get the new view controller using segue.destinationViewController.
     // Pass the selected object to the new view controller.
     }
     */
    
    
    // Store data
    
    //    UserDefaults.standard.set(userName, forKey: "userName")
    //    UserDefaults.standard.set(userContactNumber, forKey: "userContactNumber")
    //    UserDefaults.standard.set(userEmail, forKey: "userEmail")
    //    UserDefaults.standard.set(userPassword, forKey: "userPassword")
    //    UserDefaults.standard.register(defaults: ["userRank": "New IR"])
    //    UserDefaults.standard.register(defaults: ["userTeamSize": 0])
    //    UserDefaults.standard.register(defaults: ["userDRCount": 0])
    
}
