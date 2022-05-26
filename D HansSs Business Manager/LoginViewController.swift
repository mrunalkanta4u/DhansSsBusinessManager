//
//  LoginViewController.swift
//  UserLoginAndRegistration
//
//  Created by Sergey Kargopolov on 2015-01-13.
//  Copyright (c) 2015 Sergey Kargopolov. All rights reserved.
//

import UIKit
import LocalAuthentication
class LoginViewController: UIViewController {

    @IBOutlet weak var userEmailTextField: UITextField!
    @IBOutlet weak var userPasswordTextField: UITextField!
    @IBOutlet weak var fingerPrintImageView: UIImageView!
    
    override func viewDidLoad() {
        super.viewDidLoad()

//        if(){
            self.fingerPrintImageView.isHidden = !UserDefaults.standard.bool(forKey: "userTouchIDEnabled")
//        }
        // Do any additional setup after loading the view.
    }

    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Dispose of any resources that can be recreated.
    }
    
    @IBAction func fingerPrintScanTapped(_ sender: Any) {
            let authenticationContext = LAContext()
            var error: NSError?
            
            if authenticationContext.canEvaluatePolicy(.deviceOwnerAuthenticationWithBiometrics, error: &error) {
                authenticationContext.evaluatePolicy(.deviceOwnerAuthenticationWithBiometrics, localizedReason: "Touch the Touch ID sensor to unlock.", reply: { (success: Bool, error: Error?) in
                    
                    if success {
                        self.navigatetoAuthenticatedVC()
                    } else {
                        if let evaluateError = error as NSError? {
                            let message = self.errorMessageForLAErrorCode(errorCode: evaluateError.code)
                            self.showAlertViewAfterEvaluatingPolicyWithMessage(message: message)
                        }
                    }
                    
                    
                })
            } else {
                showAlertViewForNoBiometrics()
                return
            }
        }
        
        func navigatetoAuthenticatedVC() {
//            if let loggedInVC = storyboard?.instantiateViewController(withIdentifier: "LoggedInVC") {
//                DispatchQueue.main.async {
//                    self.navigationController?.pushViewController(loggedInVC, animated: true)
//                }
//            }
//
            UserDefaults.standard.set(true,forKey:"isUserLoggedIn")
            UserDefaults.standard.synchronize()
            self.dismiss(animated: true, completion:nil)
            
            
        }
        
        func showAlertViewForNoBiometrics() {
            showAlertWithTitle(title: "Error", message: "This device does not have a Touch ID sensor.")
        }
        
        func showAlertWithTitle(title: String, message: String) {
            let alertVC = UIAlertController(title: title, message: message, preferredStyle: .alert)
            let okAction = UIAlertAction(title: "OK", style: .default, handler: nil)
            
            alertVC.addAction(okAction)
            
            DispatchQueue.main.async {
                self.present(alertVC, animated: true, completion: nil)
            }
            
        }
        
        func showAlertViewAfterEvaluatingPolicyWithMessage(message: String) {
            showAlertWithTitle(title: "Error", message: message)
        }
        
        func errorMessageForLAErrorCode(errorCode: Int) -> String {
            var message = ""
            
            switch errorCode {
            case LAError.appCancel.rawValue:
                message = "Authentication was cancelled by application"
                
            case LAError.authenticationFailed.rawValue:
                message = "The user failed to provide valid credentials"
                
            case LAError.invalidContext.rawValue:
                message = "The context is invalid"
                
            case LAError.passcodeNotSet.rawValue:
                message = "Passcode is not set on the device"
                
            case LAError.systemCancel.rawValue:
                message = "Authentication was cancelled by the system"
                
            case LAError.biometryLockout.rawValue:
                message = "Too many failed attempts."
                
            case LAError.biometryNotAvailable.rawValue:
                message = "TouchID is not available on the device"
                
            case LAError.userCancel.rawValue:
                message = "The user did cancel"
                
            case LAError.userFallback.rawValue:
                message = "The user chose to use the fallback"
                
            default:
                message = "Did not find error code on LAError object"
            }
            return message
        }
    func textFieldShouldReturn(textField: UITextField) -> Bool {
        self.view.endEditing(true)
        return true;
    }
    
    override func touchesBegan(_ touches: Set<UITouch>, with event: UIEvent?) {
        self.view.endEditing(true)
    }
    
    @IBAction func loginButtonTapped(_ sender: Any) {
        
        let userEmail = userEmailTextField.text;
        let userPassword = userPasswordTextField.text;
        
        let userEmailStored = UserDefaults.standard.string(forKey: "userEmail");
        
        let userPasswordStored = UserDefaults.standard.string(forKey: "userPassword");
        
        if(userEmailStored == userEmail)
        {
            if(userPasswordStored == userPassword)
            {
                // Login is successfull
                self.navigatetoAuthenticatedVC()
 
            }
        }
    }
    
    /*
    // MARK: - Navigation

    // In a storyboard-based application, you will often want to do a little preparation before navigation
    override func prepareForSegue(segue: UIStoryboardSegue, sender: AnyObject?) {
        // Get the new view controller using segue.destinationViewController.
        // Pass the selected object to the new view controller.
    }
    */

}
