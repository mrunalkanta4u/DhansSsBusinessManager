//
//  MiscellaneousViewController.swift
//  D HansSs Biz Manager
//
//  Created by Mrunal Kanta Muduli on 24/09/17.
//  Copyright © 2017 Mrunal Kanta Muduli. All rights reserved.
//

import UIKit
import Contacts
import ContactsUI
import CoreData
import UserNotifications

class MiscellaneousViewController: UITableViewController, NSFetchedResultsControllerDelegate, CNContactPickerDelegate { //}, UIPickerViewDelegate, UIPickerViewDataSource {
    
    
    
    var detailViewController: NameListDetailViewController? = nil
    var managedObjectContext: NSManagedObjectContext? = nil
    
    var contact: [NSManagedObject] = []
    
    @IBAction func logOutButtonTapped(_ sender: Any) {
        UserDefaults.standard.set(false,forKey:"isUserLoggedIn");
        UserDefaults.standard.set(true,forKey:"isFirstLogIn");
        UserDefaults.standard.synchronize();
        
        self.performSegue(withIdentifier: "logoutView", sender: self);
        
    }
    @IBAction func add108Recipiant(_ sender: Any) {
        let entityType = CNEntityType.contacts
        let authStatus = CNContactStore.authorizationStatus(for: entityType)
        if authStatus == CNAuthorizationStatus.notDetermined
        {
            let contactStore = CNContactStore.init()
            contactStore.requestAccess(for: entityType, completionHandler: {(success,nil) in
                if success{
                    self.addTo108RecipiantList()
                }
                else{
                    print("Not Authorised")
                }
            })
        }
        else if authStatus == CNAuthorizationStatus.authorized{
            self.addTo108RecipiantList()
        }
    }
    
    @IBOutlet weak var versionTextLabel: UILabel!
    @IBOutlet weak var buildTextLabel: UILabel!
    
    
    @IBOutlet weak var userNameTextLabel: UILabel!
    @IBOutlet weak var userContactNumberTextLabel: UILabel!
    @IBOutlet weak var userEmailTextLAbel: UILabel!
    @IBOutlet weak var userProfilePictureImageView: UIImageView!
    
    @IBOutlet weak var userLocationEnabledSwitch: UISwitch!
    @IBOutlet weak var userTouchIDEnabledSwitch: UISwitch!
    @IBOutlet weak var user108MessageEnabledSwitch: UISwitch!
    
    @IBOutlet weak var userAutoBackupEnabledSwitch: UISwitch!
    @IBOutlet weak var userBackupIntervalSegmentedControl: UISegmentedControl!
    
    @IBOutlet weak var nameListDefaultContactTypeSegmentedControl: UISegmentedControl!
    
    @IBOutlet weak var user108ReminderEnabledSwitch: UISwitch!
    @IBOutlet weak var user108ModeSegmentedControl: UISegmentedControl!
    
    @IBOutlet weak var user108TimePicker: UIDatePicker!
    
    func setPreferences(){
        self.versionTextLabel.text = Bundle.main.infoDictionary!["CFBundleShortVersionString"] as? String
        self.buildTextLabel.text = Bundle.main.infoDictionary!["CFBundleVersion"] as? String
        
        //        UserDefaults.standard.synchronize()
        self.userNameTextLabel.text = UserDefaults.standard.string(forKey: "userName")
        self.userContactNumberTextLabel.text = UserDefaults.standard.string(forKey: "userContactNumber")
        self.userEmailTextLAbel.text = UserDefaults.standard.string(forKey: "userEmail")
        let imageData = UserDefaults.standard.value(forKey: "userProfilePicture") as! Data
        self.userProfilePictureImageView.image = UIImage(data: imageData)!
        
        self.userLocationEnabledSwitch.isOn = UserDefaults.standard.bool(forKey: "userLocationEnabled")
        self.userTouchIDEnabledSwitch.isOn = UserDefaults.standard.bool(forKey: "userTouchIDEnabled")
        self.user108MessageEnabledSwitch.isOn = UserDefaults.standard.bool(forKey: "user108MessageEnabled")
        self.userAutoBackupEnabledSwitch.isOn = UserDefaults.standard.bool(forKey: "userAutoBackupEnabled")
        self.userBackupIntervalSegmentedControl.selectedSegmentIndex = UserDefaults.standard.integer(forKey: "userBackupInterval")
        self.nameListDefaultContactTypeSegmentedControl.selectedSegmentIndex = UserDefaults.standard.integer(forKey: "nameListDefaultContactType")
        self.user108ReminderEnabledSwitch.isOn = UserDefaults.standard.bool(forKey: "user108ReminderEnabled")
        self.user108ModeSegmentedControl.selectedSegmentIndex = UserDefaults.standard.integer(forKey: "user108Mode")
        if let date = UserDefaults.standard.object(forKey: "user108Time") as? Date {
            self.user108TimePicker.setDate(date, animated: true)
        }
        
        
    }
    func addTo108RecipiantList   (){
        let recipiantPicker = CNContactPickerViewController.init()
        recipiantPicker.delegate = self
        self.present(recipiantPicker, animated: true, completion: nil)
    }
    
    //    @IBOutlet weak var backupIntervalPicker: UIPickerView!
    //
    //    var backupIntervalPickerData: [String] = [String]()
    
    func logOutUser()
    {
        let isUserLoggedIn = UserDefaults.standard.bool(forKey: "isUserLoggedIn")
        if(isUserLoggedIn){
            
        }
    }
    override func viewDidLoad() {
        super.viewDidLoad()
        
        
        //Asked for permission
        UNUserNotificationCenter.current().requestAuthorization(options: [.alert, .sound, .badge], completionHandler: {didAllow, error in
        })
        
        // Do any additional setup after loading the view, typically from a nib.
        
        //        self.backupIntervalPicker.delegate = self
        //        self.backupIntervalPicker.dataSource = self
        //        // Input data into the Array:
        //        backupIntervalPickerData = ["Daily", "Weekly", "Monthly", "Yearly"]
        setPreferences()
    }
    
    
    //    @IBAction func action(_ sender: Any)
    //    {
    //        let content = UNMutableNotificationContent()
    //        content.title = "How many days are there in one year"
    //        content.subtitle = "Do you know?"
    //        content.body = "Do you really know?"
    //        content.badge = 1
    //
    //        let trigger = UNTimeIntervalNotificationTrigger(timeInterval: 5, repeats: false)
    //        let request = UNNotificationRequest(identifier: "timerDone", content: content, trigger: trigger)
    //
    //        UNUserNotificationCenter.current().add(request, withCompletionHandler: nil)
    //    }
    
    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Dispose of any resources that can be recreated.
    }
    
    // The number of columns of data
    func numberOfComponents(in pickerView: UIPickerView) -> Int {
        return 1
    }
    // The number of rows of data
    //    func pickerView(_ pickerView: UIPickerView, numberOfRowsInComponent component: Int) -> Int {
    //        return backupIntervalPickerData.count
    //    }
    //
    //    // The data to return for the row and component (column) that's being passed in
    //    func pickerView(_ pickerView: UIPickerView, titleForRow row: Int, forComponent component: Int) -> String? {
    //        return backupIntervalPickerData[row]
    //    }
    
    @IBAction func importContactsToNameList(_ sender: Any) {
        let entityType = CNEntityType.contacts
        let authStatus = CNContactStore.authorizationStatus(for: entityType)
        if authStatus == CNAuthorizationStatus.notDetermined
        {
            let contactStore = CNContactStore.init()
            contactStore.requestAccess(for: entityType, completionHandler: {(success,nil) in
                if success{
                    self.openContacts()
                }
                else{
                    print("Not Authorised")
                }
            })
        }
        else if authStatus == CNAuthorizationStatus.authorized{
            self.openContacts()
        }
    }
    func openContacts(){
        let contactPicker = CNContactPickerViewController.init()
        contactPicker.delegate = self
        self.present(contactPicker, animated: true, completion: nil)
    }
    func contactPickerDidCancel(_ picker: CNContactPickerViewController) {
        picker.dismiss(animated: true){
            
        }
    }
    @IBOutlet weak var recipiantList: UILabel!
    func  recipiantPicker(_ picker: CNContactPickerViewController, didSelect contacts: [CNContact]) {
        for contact in contacts{
            let contactName = "\(contact.givenName) "//\(contact.familyName)"
            print(contactName)
            self.recipiantList.text = contactName
            
        }
    }
    func  contactPicker(_ picker: CNContactPickerViewController, didSelect contacts: [CNContact]) {
        //        print(contacts)
        for contact in contacts{
            let contactName = "\(contact.givenName) \(contact.familyName)"
            print(contactName)
            
            var emailString = ""
            if !contact.emailAddresses.isEmpty{
                emailString = (((contact.emailAddresses[0] as AnyObject).value(forKey: "labelValuePair") as AnyObject).value(forKey: "value") as! String)
                print(emailString)
            }
            var contactNumber = ""
            if !contact.phoneNumbers.isEmpty{
                contactNumber = ((((contact.phoneNumbers[0] as AnyObject).value(forKey: "labelValuePair") as AnyObject).value(forKey: "value") as AnyObject).value(forKey: "stringValue") as! String)
                print(contactNumber)
            }
            
            var contactLocation = ""
            if !contact.postalAddresses.isEmpty{
                let city = ((((contact.postalAddresses[0] as AnyObject).value(forKey: "labelValuePair") as AnyObject).value(forKey: "value") as AnyObject).value(forKey: "city") as! String)
                let state = ((((contact.postalAddresses[0] as AnyObject).value(forKey: "labelValuePair") as AnyObject).value(forKey: "value") as AnyObject).value(forKey: "state") as! String)
                let country = ((((contact.postalAddresses[0] as AnyObject).value(forKey: "labelValuePair") as AnyObject).value(forKey: "value") as AnyObject).value(forKey: "country") as! String)
                if country != ""{
                    contactLocation = "\(country)"
                    if state != ""{
                        contactLocation = "\(state), \(contactLocation)"
                    }
                    if city != ""{
                        contactLocation = "\(city), \(contactLocation)"
                    }
                }
                print(contactLocation)
            }
            
            let contactDetails = contact.note
            print(contactDetails)
            var contactImage = #imageLiteral(resourceName: "DefaultContactImage")
            if  let image = contact.imageData {
                contactImage = UIImage(data: image)!
            }
            if contactName != "" && contactNumber != "" {
                importContact (contactName: contactName, contactDetails: contactDetails,contactType: 1,contactNumber: contactNumber,contactEmail: emailString,contactLocation: contactLocation ,contactInfoDate: Date(),contactInviteDate: Date(),contactImage: contactImage)
            }
        }
    }
    
    func importContact (contactName: String, contactDetails: String, contactType: Int, contactNumber: String, contactEmail: String, contactLocation: String, contactInfoDate: Date, contactInviteDate: Date, contactImage: UIImage)
    {
        guard let appDelegate = UIApplication.shared.delegate as? AppDelegate else {
            return
        }
        let context = appDelegate.persistentContainer.viewContext
        let newContact = Contact(context: context)
        // If appropriate, configure the new managed object.
        newContact.contactName = contactName
        newContact.contactNumber = contactNumber
        newContact.contactEmail = contactEmail
        newContact.contactType = Int32(contactType)
        newContact.contactLocation = contactLocation
        newContact.contactInfoDate = contactInfoDate
        newContact.contactInviteDate = contactInviteDate
        newContact.contactDetails = contactDetails
        let imageData = UIImagePNGRepresentation(contactImage)
        newContact.contactImage = imageData
        
        // Save the context.
        do {
            try context.save()
        } catch {
            // Replace this implementation with code to handle the error appropriately.
            // fatalError() causes the application to generate a crash log and terminate. You should not use this function in a shipping application, although it may be useful during development.
            let nserror = error as NSError
            fatalError("Unresolved error \(nserror), \(nserror.userInfo)")
        }
    }
    
    
    @IBAction func locationEnabledSwitchChanged(_ sender: Any) {
        UserDefaults.standard.set(self.userLocationEnabledSwitch.isOn, forKey: "userLocationEnabled")
    }
    @IBAction func touchIDEnabledSwitchChanged(_ sender: Any) {
        UserDefaults.standard.set(self.userTouchIDEnabledSwitch.isOn, forKey: "userTouchIDEnabled")
    }
    @IBAction func send108EnabledSwitchChanged(_ sender: Any) {
        UserDefaults.standard.set(self.user108MessageEnabledSwitch.isOn, forKey: "user108MessageEnabled")
    }
    @IBAction func autoBackupEnabledSwitchChanged(_ sender: Any) {
        UserDefaults.standard.set(self.userAutoBackupEnabledSwitch.isOn, forKey: "userAutoBackupEnabled")
    }
    @IBAction func autoBackupIntervalEnabledSegmentedCtrlChanged(_ sender: Any) {
        UserDefaults.standard.set(self.userBackupIntervalSegmentedControl.selectedSegmentIndex, forKey: "userBackupInterval")
    }
    @IBAction func defaultContactTypeSegmentedCtrlChanged(_ sender: Any) {
        UserDefaults.standard.set(self.nameListDefaultContactTypeSegmentedControl.selectedSegmentIndex, forKey: "nameListDefaultContactType")
    }
    @IBAction func send108ReminderEnabledSwitchChanged(_ sender: Any) {
        UserDefaults.standard.set(self.user108ReminderEnabledSwitch.isOn, forKey: "user108ReminderEnabled")
        if(self.user108ReminderEnabledSwitch.isOn)
        {
            secheduleNotification()
        }
    }
    @IBAction func default108SendingModeSegmentedCtrlChanged(_ sender: Any) {
        UserDefaults.standard.set(self.user108ModeSegmentedControl.selectedSegmentIndex, forKey: "user108Mode")
    }
    @IBAction func send108TimeChanged(_ sender: Any) {
        
        // Store value using User Defaults
        let currentDate = self.user108TimePicker.date
        UserDefaults.standard.set(currentDate, forKey: "user108Time")
        if(self.user108ReminderEnabledSwitch.isOn)
        {
            secheduleNotification()
        }
    }
    
    func secheduleNotification(){
        let content = UNMutableNotificationContent()
        
//        content.title = "Its time to send 108 Messages"
////        content.subtitle = "Its time to send 108 Messages."
//        content.body = "Keep your leaders updated about your business today."
//        content.badge = 1
//
//        let trigger = UNTimeIntervalNotificationTrigger(timeInterval: 5, repeats: false)
//        let request = UNNotificationRequest(identifier: "timerDome", content: content, trigger: trigger)
//
//        UNUserNotificationCenter.current().add(request, withCompletionHandler: nil)
//
//
        
        
        let notification = UILocalNotification()
        
        
//        let content = UNMutableNotificationContent()
//        content.badge = 10 // your badge count

        /* Time and timezone settings */
//        if let date = UserDefaults.standard.object(forKey: "user108Time") as? Date {
//            notification.fireDate =  date
//        }
//        else{
//             notification.fireDate = NSDate(timeIntervalSinceNow: 1.0) as Date
//        }
        if let date = UserDefaults.standard.object(forKey: "user108Time") as? Date {
            let dateFormatter = DateFormatter()
            dateFormatter.dateFormat = "d MMMM yyyy, h:mm a"
            
            let infoDate = dateFormatter.string(from: date)
            
            print (infoDate)
        }
        notification.fireDate = self.user108TimePicker.date as Date  //NSDate(timeIntervalSinceNow: 8.0) as Date//
//        print (notification.fireDate!)
        notification.repeatInterval = NSCalendar.Unit.day
        notification.timeZone = NSCalendar.current.timeZone

        notification.alertTitle = "Its time to send 108 Messages"
        notification.alertBody = "Keep your leaders updated about your business today."
//        notification.alertAction =
        /* Action settings */
        notification.hasAction = true
        notification.alertAction = "View"
//        notification.applicationIconBadgeNumber = 0
//
//        /* Badge settings */
//        notification.applicationIconBadgeNumber =
//            UIApplication.shared.applicationIconBadgeNumber - 1
//UIApplication.shared.applicationIconBadgeNumber = 0
        //        /* Additional information, user info */
        notification.userInfo = [
            "Key 1" : "Value 1",
            "Key 2" : "Value 2"
        ]
//
        /* Schedule the notification */
        UIApplication.shared.scheduleLocalNotification(notification)
    
        
        
        
        
        
        
        
        
        
        
        
    }
    
}

//
//extension ViewController: UNUserNotificationCenterDelegate {
//    func userNotificationCenter(_ center: UNUserNotificationCenter, willPresent notification: UNNotification, withCompletionHandler completionHandler: (UNNotificationPresentationOptions) -> Void) {
//        // some other way of handling notification
//        completionHandler([.alert, .sound])
//    }
//    func userNotificationCenter(_ center: UNUserNotificationCenter, didReceive response: UNNotificationResponse, withCompletionHandler completionHandler: () -> Void) {
//        switch response.actionIdentifier {
//        case "answerOne":
//            imageView.image = UIImage(named: "wrong")
//        case "answerTwo":
//            imageView.image = UIImage(named: "correct")
//        case "clue":
//            let alert = UIAlertController(title: "Hint", message: "The answer is greater than 29", preferredStyle: .alert)
//            let action = UIAlertAction(title: "Thanks!", style: .default, handler: nil)
//            alert.addAction(action)
//            present(alert, animated: true, completion: nil)
//        default:
//            break
//        }
//        completionHandler()
//
//    }
//}


