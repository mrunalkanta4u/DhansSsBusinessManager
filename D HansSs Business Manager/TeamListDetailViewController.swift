//
//  DetailViewController.swift
//  D HansSs Business Manager
//
//  Created by Mrunal Kanta Muduli on 29/09/17.
//  Copyright © 2017 Mrunal Kanta Muduli. All rights reserved.
//

import UIKit

class TeamListDetailViewController: UIViewController {
    @IBOutlet weak var teamMemberNameTextLabel: UILabel!
    @IBOutlet weak var teamMemberIDTextLabel: UILabel!
    @IBOutlet weak var teamMemberIsActiveLabel: UILabel!
    @IBOutlet weak var teamMemberIsDirectLabel: UILabel!
    @IBOutlet weak var teamMemberTicketSizeLabel: UILabel!
    @IBOutlet weak var teamMemberPassword1TextLabel: UILabel!
    @IBOutlet weak var teamMemberPassword2TextLabel: UILabel!
    @IBOutlet weak var teamMemberSequrityDetailsTextLabel: UILabel!
    
    var indexPath: IndexPath? = nil

    func configureView() {
        // Update the user interface for the detail item.
        if let detail = detailItem {
            if let label = teamMemberIDTextLabel {
                label.text = detail.teamMemberID!.description
            }
            if let label = teamMemberNameTextLabel {
                label.text = detail.teamMemberName!.description
            }
            if let label = teamMemberPassword1TextLabel {
                label.text = detail.teamMemberPassword1!.description
            }
            if let label = teamMemberPassword2TextLabel {
                label.text = detail.teamMemberPassword2!.description
            }
            if let label = teamMemberSequrityDetailsTextLabel {
                label.text = detail.teamMemberSecurityInfo!.description
            }
            if let label = teamMemberIsActiveLabel {
                label.text = detail.teamMemberIsActive.description
            }
            if let label = teamMemberIsDirectLabel {
                label.text = detail.teamMemberIsDR.description
            }
            if let label = teamMemberTicketSizeLabel {
                label.text = detail.teamMemberTicketSize.description
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

    var detailItem: TeamMember? {
        didSet {
            // Update the view.
            configureView()
        }
    }

    override func prepare(for segue: UIStoryboardSegue, sender: Any?) {
        if segue.identifier == "editTeamMember" {
            guard let viewController = segue.destination as? AddTeamViewController else { return }
            viewController.titleText = "Edit Team Member"
            viewController.teamMember = self.detailItem
            viewController.indexPathForTeam = self.indexPath!
        }
    }

}

