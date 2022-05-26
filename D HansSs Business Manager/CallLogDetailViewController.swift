//
//  DetailViewController.swift
//  D HansSs Business Manager
//
//  Created by Mrunal Kanta Muduli on 29/09/17.
//  Copyright © 2017 Mrunal Kanta Muduli. All rights reserved.
//

import UIKit

class CallLogDetailViewController: UIViewController {
    @IBOutlet weak var callProspectNameTextLabel: UILabel!
    @IBOutlet weak var callTypeTextLabel: UILabel!
    @IBOutlet weak var callTime: UILabel!
    @IBOutlet weak var callDetails: UILabel!
    
    var callTypeLabel: [String] = ["Conect Up Call","Prospecting Call","Information Call","Invite Call"]
    var indexPath: IndexPath? = nil

    func configureView() {
        let dateFormatter = DateFormatter()
        dateFormatter.dateFormat = "d MMMM yyyy, h:mm a"
        // Update the user interface for the detail item.
        if let detail = detailItem {
            if let label = callProspectNameTextLabel {
                label.text = detail.callProspectName!.description
            }
            if let label = callTypeTextLabel {
                label.text = callTypeLabel[Int(detail.callType)]
            }
            if let label = callTime {
                let callDateTime = dateFormatter.string(from: detail.callTime!)
                label.text = callDateTime.description
            }
            if let label = callDetails {
                label.text = detail.callRemark!.description
            }

        }
    }

    override func viewDidLoad() {
        super.viewDidLoad()
        // Do any additional setup after loading the view, typically from a nib.
        configureView()
    }

    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Dispose of any resources that can be recreated.
    }

    var detailItem: CallLog? {
        didSet {
            // Update the view.
            configureView()
        }
    }

    override func prepare(for segue: UIStoryboardSegue, sender: Any?) {
        if segue.identifier == "editActivity" {
            guard let viewController = segue.destination as? AddCallLogViewController else { return }
            viewController.titleText = "Edit Activity"
            viewController.callLog = self.detailItem
            viewController.indexPathForCallLog = self.indexPath!
        }
    }

}

