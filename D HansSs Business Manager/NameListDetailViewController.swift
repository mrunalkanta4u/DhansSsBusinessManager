//
//  DetailViewController.swift
//  D HansSs Business Manager
//
//  Created by Mrunal Kanta Muduli on 29/09/17.
//  Copyright © 2017 Mrunal Kanta Muduli. All rights reserved.
//

import UIKit

class NameListDetailViewController: UIViewController {

    @IBOutlet weak var contactNameLabel: UILabel!
    @IBOutlet weak var contactImage: UIImageView!
    @IBOutlet weak var contactLocationLabel: UILabel!
    @IBOutlet weak var contactNumberLabel: UILabel!
    @IBOutlet weak var contactEmailLabel: UILabel!
    @IBOutlet weak var contactDetailsLabel: UILabel!
    @IBOutlet weak var contactTypeLabel: UILabel!
    @IBOutlet weak var contactInfoDateLabel: UILabel!
    @IBOutlet weak var contactInviteDateLabel: UILabel!
    var contactType = ["Hot","Warm","Cold"]
    var indexPath: IndexPath? = nil

    func configureView() {
        let dateFormatter = DateFormatter()
        dateFormatter.dateFormat = "d MMMM yyyy, h:mm a"
        // Update the user interface for the detail item.
        if let detail = detailItem {
            if let label = contactNameLabel {
                label.text = detail.contactName!
            }
            if let label = contactLocationLabel {
                label.text = detail.contactLocation!
            }
            if let label = contactNumberLabel {
                label.text = detail.contactNumber!
            }
            if let label = contactEmailLabel {
                label.text = detail.contactEmail!
            }
            if let label = contactDetailsLabel {
                label.text = detail.contactDetails!
            }
            if let label = contactInfoDateLabel {
                
                let infoDate = dateFormatter.string(from: detail.contactInfoDate!)
                label.text = infoDate.description
            }
            if let label = contactTypeLabel {
                label.text = contactType[Int(detail.contactType)]
            }
            if let label = contactInviteDateLabel {
                let inviteDate = dateFormatter.string(from: detail.contactInviteDate!)
                label.text = inviteDate.description
            }
            if let image = contactImage {
                image.image = UIImage(data: detail.contactImage!)
            }
        }
    }

    override func viewDidLoad() {
        super.viewDidLoad()
        // Do any additional setup after loading the view, typically from a nib.
        configureView()
        contactImage.setRounded()
    }

    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Dispose of any resources that can be recreated.
    }

    var detailItem: Contact? {
        didSet {
            // Update the view.
            configureView()
        }
    }

    override func prepare(for segue: UIStoryboardSegue, sender: Any?) {
        if segue.identifier == "editContact" {
            guard let viewController = segue.destination as? AddNameViewController else { return }
            viewController.titleText = "Edit Contact"
            viewController.contact = self.detailItem
            viewController.indexPathForName = self.indexPath!
        }
    }
    @IBAction func callPhoneNumber(_ sender: UILabel) {
        if  let phoneNumber = contactNumberLabel.text{
             let phoneNumberValidator = phoneNumber.isPhoneNumber
            if(phoneNumberValidator){
                if let phoneCallURL:NSURL = NSURL(string:"tel://\(String(describing: phoneNumber))") {
                    let application:UIApplication = UIApplication.shared
                    if (application.canOpenURL(phoneCallURL as URL)) {
                        application.openURL(phoneCallURL as URL);
                    }
                }
            }
        }
    }
    
}
//extension UIImageView {
//    
//    func setRounded() {
//        self.layer.cornerRadius = (self.frame.width / 2) //instead of let radius = CGRectGetWidth(self.frame) / 2
//        self.layer.masksToBounds = true
//    }
//}
extension String {
    var isPhoneNumber: Bool {
        do {
            let detector = try NSDataDetector(types: NSTextCheckingResult.CheckingType.phoneNumber.rawValue)
            let matches = detector.matches(in: self, options: [], range: NSMakeRange(0, self.characters.count))
            if let res = matches.first {
                return res.resultType == .phoneNumber && res.range.location == 0 && res.range.length == self.characters.count
            } else {
                return false
            }
        } catch {
            return false
        }
    }
}
