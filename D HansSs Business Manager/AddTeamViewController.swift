//
//  AddTeamViewController.swift
//  D HansSs Business Manager
//
//  Created by Mrunal Kanta Muduli on 29/09/17.
//  Copyright © 2017 Mrunal Kanta Muduli. All rights reserved.
//

import UIKit

class AddTeamViewController: UIViewController, UINavigationControllerDelegate  {
    var titleText = "Add Team Member"
    var teamMember: TeamMember? = nil
//    let context = self.fetchedResultsController.managedObjectContext
//    var newTeam = Team(context: context)
//
    var indexPathForTeam: IndexPath? = nil
    override func viewDidLoad() {
        super.viewDidLoad()
        titleLabel.text = titleText

        if let teamMember = teamMember {
            teamMemberNameTextField.text = teamMember.teamMemberName
            teamMemberIDTextField.text = teamMember.teamMemberID
            teamMemberTicketSize.text = String(teamMember.teamMemberTicketSize)
            teamMemberIsActiveSwitch.isOn = teamMember.teamMemberIsActive
            teamMemberIsDRSwitch.isOn = teamMember.teamMemberIsDR
            teamMemberPassword1TextField.text = teamMember.teamMemberPassword1
            teamMemberPassword2TextField.text = teamMember.teamMemberPassword2
            teamMemberSequrityQATextField.text = teamMember.teamMemberSecurityInfo
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
    @IBOutlet weak var teamMemberNameTextField: UITextField!
    @IBOutlet weak var teamMemberIDTextField: UITextField!
    @IBOutlet weak var teamMemberTicketSize: UITextField!
    @IBOutlet weak var teamMemberIsActiveSwitch: UISwitch!
    @IBOutlet weak var teamMemberIsDRSwitch: UISwitch!
    @IBOutlet weak var teamMemberPassword1TextField: UITextField!
    @IBOutlet weak var teamMemberPassword2TextField: UITextField!
    @IBOutlet weak var teamMemberSequrityQATextField: UITextField!

    
    @IBAction func close(_ sender: Any) {
        teamMemberNameTextField.text = nil
        teamMemberIDTextField.text = nil
        performSegue(withIdentifier: "unwindToTeamList", sender: self)
    }
    @IBAction func saveAndClose(_ sender: Any) {
        performSegue(withIdentifier: "unwindToTeamList", sender: self)
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
