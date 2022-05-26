//
//  RegisterPageViewController.swift
//  UserLoginAndRegistration
//
//  Created by Sergey Kargopolov on 2015-01-13.
//  Copyright (c) 2015 Sergey Kargopolov. All rights reserved.
//

import UIKit
//import Parse
 

class RegisterPageViewController: UIViewController, UIImagePickerControllerDelegate, UINavigationControllerDelegate  {

    @IBOutlet weak var userNameTextField: UITextField!
    @IBOutlet weak var userContactNumberTextField: UITextField!
    @IBOutlet weak var userEmailTextField: UITextField!
    @IBOutlet weak var userPasswordTextField: UITextField!
    @IBOutlet weak var repeatPasswordTextField: UITextField!
    
    @IBOutlet weak var userProfilePicture: UIImageView!
    override func viewDidLoad() {
        super.viewDidLoad()

        // Do any additional setup after loading the view.
    }

    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Dispose of any resources that can be recreated.
    }
    
    func textFieldShouldReturn(textField: UITextField) -> Bool {
        self.view.endEditing(true)
        return true;
    }
    
    override func touchesBegan(_ touches: Set<UITouch>, with event: UIEvent?) {
        self.view.endEditing(true)
    }
    
    @IBAction func profilePictureTapped(_ sender: Any) {
        
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
            userProfilePicture.image = image
        }
        else
        {
            //Error message
        }
        
        self.dismiss(animated: true, completion: nil)
    }

    @IBAction func registerButtonTapped(_ sender: Any) {
        
        let userName = userNameTextField.text
        let userContactNumber = userContactNumberTextField.text
        let userEmail = userEmailTextField.text
        let userPassword = userPasswordTextField.text
        let userRepeatPassword = repeatPasswordTextField.text
        
        // Check for empty fields
        if((userName?.isEmpty)! || (userContactNumber?.isEmpty)! || (userEmail?.isEmpty)! || (userPassword?.isEmpty)! || (userRepeatPassword?.isEmpty)!)
        {
            
            // Display alert message 
            
            displayMyAlertMessage(userMessage: "All fields are required")
            
            return
        }
        
        //Check if passwords match 
        if(userPassword != userRepeatPassword)
        {
           // Display an alert message 
            displayMyAlertMessage(userMessage: "Passwords do not match")
            return
        
        }
      
        // Store data
        
        UserDefaults.standard.set(userName, forKey: "userName")
        UserDefaults.standard.set(userContactNumber, forKey: "userContactNumber")
        UserDefaults.standard.set(userEmail, forKey: "userEmail")
        UserDefaults.standard.set(userPassword, forKey: "userPassword")
        UserDefaults.standard.set("New IR", forKey: "userRank")
        UserDefaults.standard.set(0, forKey: "userTeamSize")
        UserDefaults.standard.set(0, forKey: "userDRCount")
//        Register Default Image :
        
//        UserDefaults.standard.register(defaults: ["key":UIImageJPEGRepresentation(userProfilePicture.image, 100)!])
//        Save Image :
        
        UserDefaults.standard.set(UIImageJPEGRepresentation(userProfilePicture.image!, 100), forKey: "userProfilePicture")
//        Load Image :
        
//        let imageData = UserDefaults.standard.value(forKey: "key") as! Data
//        let imageFromData = UIImage(data: imageData)!

        
        UserDefaults.standard.synchronize()
        
    
        print("User successfully registered")
       
            // Display alert message with confirmation.
        let myAlert = UIAlertController(title:"Alert", message:"Registration is successful. Thank you!", preferredStyle: UIAlertControllerStyle.alert)
            
        let okAction = UIAlertAction(title:"Ok", style:UIAlertActionStyle.default){ action in
            self.dismiss(animated: true, completion:nil)
            }
            
            myAlert.addAction(okAction)
        self.present(myAlert, animated:true, completion:nil)
                
        //}
        
    }
    
    
    func displayMyAlertMessage(userMessage:String)
    {
      
        let myAlert = UIAlertController(title:"Alert", message:userMessage, preferredStyle: UIAlertControllerStyle.alert)
        
        let okAction = UIAlertAction(title:"Ok", style:UIAlertActionStyle.default, handler:nil)
        
        myAlert.addAction(okAction)

        self.present(myAlert, animated:true, completion:nil)
        
    }
 
    @IBAction func iHaveAnAccountButtonTapped(_ sender: Any) {
        self.dismiss(animated: true, completion: nil)
    }

}
