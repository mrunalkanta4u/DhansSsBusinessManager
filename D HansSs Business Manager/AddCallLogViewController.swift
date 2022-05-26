
//
//  AddCallLogViewController.swift
//  D HansSs Business Manager
//
//  Created by Mrunal Kanta Muduli on 29/09/17.
//  Copyright © 2017 Mrunal Kanta Muduli. All rights reserved.
//

import UIKit
import CoreData

class AddCallLogViewController: UIViewController, UINavigationControllerDelegate, UITextFieldDelegate, UITableViewDelegate, UITableViewDataSource, NSFetchedResultsControllerDelegate  {
    var titleText = "New Activity"
    var callLog: CallLog? = nil
    //    let context = self.fetchedResultsController.managedObjectContext
    //    var newTeam = Team(context: context)
    //
    //    @IBOutlet weak var addButton: UIButton!
    //
    var autoCompletePossibilities: [Contact] = []
    //    var autoCompletePossibilities = ["mrunal", "mridul", "gunjan"]
    var autocomplete = [Contact]()
    
    var indexPathForCallLog: IndexPath? = nil
    override func viewDidLoad() {
        super.viewDidLoad()
        callProspectNameTextField.delegate = self
        callProspectNameListTableView.delegate = self
        callProspectNameListTableView.isHidden = true
        callProspectNameTextField.clearButtonMode = UITextFieldViewMode.whileEditing
        callDatePicker.maximumDate = Date()
        titleLabel.text = titleText
        
        if let callLog = callLog {
            callProspectNameTextField.text = callLog.callProspectName
            callTypeSegmentedControl.selectedSegmentIndex = Int(callLog.callType)
            callDatePicker.date = callLog.callTime!
            callDetailsTextField.text = callLog.callRemark
            ///TODO
        }
        
        fetchNameListData()
        // Do any additional setup after loading the view.
    }
    func fetchNameListData(){
        
        autoCompletePossibilities.removeAll()
        let context = (UIApplication.shared.delegate as! AppDelegate).persistentContainer.viewContext
        let fetchRequest = NSFetchRequest<NSFetchRequestResult>(entityName: "Contact")
        
        do {
            let results = try context.fetch(fetchRequest)
            let  nameList = results as! [Contact]
            
            for _nameList in nameList {
                print(_nameList.contactName!)
                autoCompletePossibilities.append(_nameList)
            }
        }catch let err as NSError {
            print(err.debugDescription)
        }
    }
    
    
    func textFieldShouldReturn(_ textField: UITextField) -> Bool {
        self.view.endEditing(true)
        callProspectNameListTableView.isHidden = true
        return true;
    }
    
    override func touchesBegan(_ touches: Set<UITouch>, with event: UIEvent?) {
        self.view.endEditing(true)
        callProspectNameListTableView.isHidden = true
    }
    
    
    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Dispose of any resources that can be recreated.
    }
    
    
    @IBOutlet weak var titleLabel: UILabel!
    @IBOutlet weak var callProspectNameTextField: UITextField!
    @IBOutlet weak var callDatePicker: UIDatePicker!
    @IBOutlet weak var callTypeSegmentedControl: UISegmentedControl!
    @IBOutlet weak var callDetailsTextField: UITextField!
    @IBOutlet weak var callProspectNameListTableView: UITableView!
    
    
    @IBAction func close(_ sender: Any) {
        callProspectNameTextField.text = nil
        callDetailsTextField.text = nil
        performSegue(withIdentifier: "unwindToCallLog", sender: self)
    }
    @IBAction func saveAndClose(_ sender: Any) {
        performSegue(withIdentifier: "unwindToCallLog", sender: self)
    }
    
    
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return autocomplete.count
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = callProspectNameListTableView.dequeueReusableCell(withIdentifier: "cell", for: indexPath) as UITableViewCell
        let index = indexPath.row as Int
        cell.textLabel?.text = autocomplete[index].contactName
        cell.detailTextLabel?.text = autocomplete[index].contactNumber
        cell.imageView?.image = UIImage(data: autocomplete[index].contactImage!)
        cell.imageView!.setRounded()
        return cell
    }
    
    func textField(_ textField: UITextField, shouldChangeCharactersIn range: NSRange, replacementString string: String) -> Bool {
        callProspectNameListTableView.isHidden = false
        let substring = (textField.text! as NSString).replacingCharacters(in: range, with: string)
        serachAutoCompleteEntriesWithSubstring(substring: substring)
        
        if autocomplete.isEmpty {
            self.callProspectNameListTableView.isHidden = true
        }
        else {
            self.callProspectNameListTableView.isHidden = false
        }
        return true
    }
    
    func serachAutoCompleteEntriesWithSubstring (substring: String){
        autocomplete.removeAll(keepingCapacity: false)
        for key in autoCompletePossibilities{
            let myString:NSString! = key.contactName as! NSString
            let substringRange :NSRange! = myString.range(of: substring)
            if(substringRange.location == 0){
                autocomplete.append(key)
            }
        }
        callProspectNameListTableView.reloadData()
    }
    
    func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
        let selectedCell: UITableViewCell = callProspectNameListTableView.cellForRow(at: indexPath)!
        callProspectNameTextField.text = selectedCell.textLabel!.text!
        callProspectNameListTableView.isHidden = true
    }
    
    func clearTextFieldAfterAddingToArray(){
        callProspectNameTextField.text = ""
    }
    
    //    @IBAction func addButtonTapped(_ sender: UIButton) {
    //        addToArray(textField: callProspectNameTextField)
    //        clearTextFieldAfterAddingToArray()
    //    }
    //    func addToArray(textField: UITextField) {
    //        if textField == textField {
    //            let textToAdd = textField.text ?? ""
    //            autoCompletePossibilities.append(textToAdd)
    //            print("it worked")
    //        }
    //    }
    
    //    override func viewWillAppear(_ animated: Bool) {
    //        if autoCompletePossibilities.isEmpty {
    //            self.callProspectNameListTableView.isHidden = true
    //        }
    //        else {
    //            self.callProspectNameListTableView.isHidden = false
    //        }
    //    }
    //
    //    callProspectNameTextField.addTarget(self, action: #selector(textFieldDidChange(_:)), for: .editingChanged)
    //    and
    //
    //    func textFieldDidChange(_ textField: UITextField) {
    //        callProspectNameListTableView.isHidden = false
    //    }
    /*
     // MARK: - Navigation
     
     // In a storyboard-based application, you will often want to do a little preparation before navigation
     override func prepare(for segue: UIStoryboardSegue, sender: Any?) {
     // Get the new view controller using segue.destinationViewController.
     // Pass the selected object to the new view controller.
     }
     */
    
}

