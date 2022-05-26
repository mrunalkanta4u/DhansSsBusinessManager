//
//  CallLogTableViewCell.swift
//  D HansSs Business Manager
//
//  Created by Mrunal Kanta Muduli on 13/10/17.
//  Copyright © 2017 Mrunal Kanta Muduli. All rights reserved.
//

import UIKit

class CallLogTableViewCell: UITableViewCell {

    @IBOutlet weak var callTypeImage: UIImageView!
    @IBOutlet weak var prospectNameTextLabel: UILabel!
    @IBOutlet weak var callTimeTextLabel: UILabel!
    @IBOutlet weak var callDetailsTextLabel: UILabel!
    //    @IBOutlet weak var callInfoButton: UIButton!
    
    
//    @IBAction func infoButtonTapped(_ sender: Any) {
////        if let indexPath = tableView.indexPathForSelectedRow {
////            var object : CallLog? = nil
////            object = fetchedResultsController.object(at: indexPath)
////            let controller = (segue.destination as! UINavigationController).topViewController as! CallLogDetailViewController
////            controller.detailItem = object
////            controller.navigationItem.leftBarButtonItem = splitViewController?.displayModeButtonItem
////            controller.navigationItem.leftItemsSupplementBackButton = true
////            controller.indexPath = indexPath
////        }
//    }
    
    
    override func awakeFromNib() {
        super.awakeFromNib()
        // Initialization code
    }

    override func setSelected(_ selected: Bool, animated: Bool) {
        super.setSelected(selected, animated: animated)

        // Configure the view for the selected state
    }

}
