//
//  AddNameViewController.swift
//  D HansSs Business Manager
//
//  Created by Mrunal Kanta Muduli on 29/09/17.
//  Copyright © 2017 Mrunal Kanta Muduli. All rights reserved.
//

import UIKit

class AddNameViewController: UIViewController, UIImagePickerControllerDelegate, UINavigationControllerDelegate  {
    var titleText = "Add Contact"
    var contact: Contact? = nil
//    let context = self.fetchedResultsController.managedObjectContext
//    var newName = Name(context: context)
//
    var indexPathForName: IndexPath? = nil
    override func viewDidLoad() {
        super.viewDidLoad()
//        imagePicker.delegate = self
        titleLabel.text = titleText
        self.contactTypeSegmentedControl.selectedSegmentIndex = UserDefaults.standard.integer(forKey: "nameListDefaultContactType")
//        contactImageView.frame.width = contactImageView.frame.height = 200
        contactImageView.setRounded()
        if let contact = contact {
            contactNameTextField.text = contact.contactName
            contactNumberTextField.text = contact.contactNumber
            contactEmailTextField.text = contact.contactEmail
            contactLocationTextField.text = contact.contactLocation
            contactDetailTextField.text = contact.contactDetails
            contactTypeSegmentedControl.selectedSegmentIndex = Int(contact.contactType)
            contactImageView.image =  UIImage(data: contact.contactImage!)
        }

        // Do any additional setup after loading the view.
    }
    
    func textFieldShouldReturn(textField: UITextField) -> Bool {
        self.view.endEditing(true)
        return true;
    }

    override func touchesBegan(_ touches: Set<UITouch>, with event: UIEvent?) {
        self.view.endEditing(true)
    }
    
    
    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Dispose of any resources that can be recreated.
    }
    
    @IBOutlet weak var titleLabel: UILabel!
    @IBOutlet weak var contactImageView: UIImageView!
    @IBOutlet weak var contactNameTextField: UITextField!
    @IBOutlet weak var contactLocationTextField: UITextField!
    @IBOutlet weak var contactNumberTextField: UITextField!
    @IBOutlet weak var contactEmailTextField: UITextField!
    @IBOutlet weak var contactDetailTextField: UITextField!
    @IBOutlet weak var contactTypeSegmentedControl: UISegmentedControl!
    
    @IBAction func close(_ sender: Any) {
        contactNameTextField.text = nil
        contactNumberTextField.text = nil
        contactImageView.image = nil
//        NameDatePicker.date = nil
        contactTypeSegmentedControl.selectedSegmentIndex = -1
        
        performSegue(withIdentifier: "unwindToNameList", sender: self)
    }
    @IBAction func saveAndClose(_ sender: Any) {
        performSegue(withIdentifier: "unwindToNameList", sender: self)
    }
    
    
    
    
    // MARK: - UIImagePickerControllerDelegate Methods

    
    @IBAction func importImageTapped(_ sender: Any) {
        
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
            contactImageView.image = image
        }
        else
        {
            //Error message
        }

        self.dismiss(animated: true, completion: nil)
    }


//    func imagePickerControllerDidCancel(_ picker: UIImagePickerController) {
//        self.dismiss(animated: true, completion: nil)
//    }
//
    /*
    // MARK: - Navigation

    // In a storyboard-based application, you will often want to do a little preparation before navigation
    override func prepare(for segue: UIStoryboardSegue, sender: Any?) {
        // Get the new view controller using segue.destinationViewController.
        // Pass the selected object to the new view controller.
    }
    */

}

