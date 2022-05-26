//
//  MasterViewController.swift
//  D HansSs Business Manager
//
//  Created by Mrunal Kanta Muduli on 29/09/17.
//  Copyright © 2017 Mrunal Kanta Muduli. All rights reserved.
//

import UIKit
import CoreData

class NameListViewController: UITableViewController, NSFetchedResultsControllerDelegate, UISearchResultsUpdating {

    
    var detailViewController: NameListDetailViewController? = nil
    var managedObjectContext: NSManagedObjectContext? = nil
    
    let searchController = UISearchController(searchResultsController: nil)
    var searchPredicate: NSPredicate? = nil
    var filteredObjects : [Contact]? = nil
    
    func filterContentForSearchText(searchText: String, scope: String = "All" )
    {
        searchPredicate = NSPredicate(format: "contactName contains[c] %@", searchText)
        filteredObjects = self.fetchedResultsController.fetchedObjects?.filter() {
            return self.searchPredicate!.evaluate(with: $0)
            } as [Contact]?
        self.tableView.reloadData()
    }
    func updateSearchResults(for searchController: UISearchController) {
        filterContentForSearchText(searchText: searchController.searchBar.text!)
    }
    
    override func viewDidLoad() {
        super.viewDidLoad()
        // Do any additional setup after loading the view, typically from a nib.
        navigationItem.leftBarButtonItem = editButtonItem
        
        if let split = splitViewController {
            let controllers = split.viewControllers
            detailViewController = (controllers[controllers.count-1] as! UINavigationController).topViewController as? NameListDetailViewController
        }
        
        // UISearchController setup
        searchController.dimsBackgroundDuringPresentation = false
        searchController.searchResultsUpdater = self
        self.tableView.tableHeaderView = searchController.searchBar
        self.definesPresentationContext = true
    }
    
    override func viewWillAppear(_ animated: Bool) {
        clearsSelectionOnViewWillAppear = splitViewController!.isCollapsed
        super.viewWillAppear(animated)
    }
    
    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Dispose of any resources that can be recreated.
    }
    

    // MARK: - Segues
    
    override func prepare(for segue: UIStoryboardSegue, sender: Any?) {
        if segue.identifier == "showContactDetail" {
            if let indexPath = tableView.indexPathForSelectedRow {
                var object : Contact? = nil
                if searchController.isActive && searchController.searchBar.text != ""
                {
                    object = filteredObjects?[indexPath.row]
                }
                else{
                    object = fetchedResultsController.object(at: indexPath)
                }
                let controller = (segue.destination as! UINavigationController).topViewController as! NameListDetailViewController
                controller.detailItem = object
                controller.navigationItem.leftBarButtonItem = splitViewController?.displayModeButtonItem
                controller.navigationItem.leftItemsSupplementBackButton = true
                controller.indexPath = indexPath
            }
        }
    }
    
    
    // MARK: - Table View
    
    override func numberOfSections(in tableView: UITableView) -> Int {
        return fetchedResultsController.sections?.count ?? 0
    }
    
    override func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        if searchController.isActive && searchController.searchBar.text != ""{
            return (filteredObjects?.count)!
        }
        let sectionInfo = fetchedResultsController.sections![section]
        return sectionInfo.numberOfObjects
    }
    
    override func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = tableView.dequeueReusableCell(withIdentifier: "ContactCell", for: indexPath)
        cell.imageView?.setRounded()

        var contact : Contact? = nil
        if searchController.isActive && searchController.searchBar.text != ""{
            contact = filteredObjects?[indexPath.row]
        }else{
            contact = fetchedResultsController.object(at: indexPath)
        }
        configureCell(cell, withName: contact!)
        return cell
    }
    
    override func tableView(_ tableView: UITableView, canEditRowAt indexPath: IndexPath) -> Bool {
        // Return false if you do not want the specified item to be editable.
        return true
    }
    
    override func tableView(_ tableView: UITableView, commit editingStyle: UITableViewCellEditingStyle, forRowAt indexPath: IndexPath) {
        if editingStyle == .delete {
            let context = fetchedResultsController.managedObjectContext
            context.delete(fetchedResultsController.object(at: indexPath))
            
            do {
                try context.save()
            } catch {
                // Replace this implementation with code to handle the error appropriately.
                // fatalError() causes the application to generate a crash log and terminate. You should not use this function in a shipping application, although it may be useful during development.
                let nserror = error as NSError
                fatalError("Unresolved error \(nserror), \(nserror.userInfo)")
            }
        }
    }
    
    func configureCell(_ cell: UITableViewCell, withName contact: Contact) {
        cell.textLabel!.text = contact.contactName!.description
        cell.detailTextLabel?.text = contact.contactNumber!.description
        cell.imageView?.image = UIImage(data: contact.contactImage!)
//        cell.imageView?.setRounded()
        //TODO - save image saving of default type again and again
    }
    
    // MARK: - Fetched results controller
    
    var fetchedResultsController: NSFetchedResultsController<Contact> {
        if _fetchedResultsController != nil {
            return _fetchedResultsController!
        }
        
        let fetchRequest: NSFetchRequest<Contact> = Contact.fetchRequest()
        
        // Set the batch size to a suitable number.
        fetchRequest.fetchBatchSize = 20
        
        // Edit the sort key as appropriate.
        let sortDescriptor = NSSortDescriptor(key: "contactName", ascending: false)
        
        fetchRequest.sortDescriptors = [sortDescriptor]
        
        // Edit the section name key path and cache name if appropriate.
        // nil for section name key path means "no sections".
        let aFetchedResultsController = NSFetchedResultsController(fetchRequest: fetchRequest, managedObjectContext: self.managedObjectContext!, sectionNameKeyPath: nil, cacheName: "NameList")
        aFetchedResultsController.delegate = self
        _fetchedResultsController = aFetchedResultsController
        
        do {
            try _fetchedResultsController!.performFetch()
        } catch {
            // Replace this implementation with code to handle the error appropriately.
            // fatalError() causes the application to generate a crash log and terminate. You should not use this function in a shipping application, although it may be useful during development.
            let nserror = error as NSError
            fatalError("Unresolved error \(nserror), \(nserror.userInfo)")
        }
        
        return _fetchedResultsController!
    }
    var _fetchedResultsController: NSFetchedResultsController<Contact>? = nil
    
    func controllerWillChangeContent(_ controller: NSFetchedResultsController<NSFetchRequestResult>) {
        tableView.beginUpdates()
    }
    
    func controller(_ controller: NSFetchedResultsController<NSFetchRequestResult>, didChange sectionInfo: NSFetchedResultsSectionInfo, atSectionIndex sectionIndex: Int, for type: NSFetchedResultsChangeType) {
        switch type {
        case .insert:
            tableView.insertSections(IndexSet(integer: sectionIndex), with: .fade)
        case .delete:
            tableView.deleteSections(IndexSet(integer: sectionIndex), with: .fade)
        default:
            return
        }
    }
    
    func controller(_ controller: NSFetchedResultsController<NSFetchRequestResult>, didChange anObject: Any, at indexPath: IndexPath?, for type: NSFetchedResultsChangeType, newIndexPath: IndexPath?) {
        switch type {
        case .insert:
            tableView.insertRows(at: [newIndexPath!], with: .fade)
        case .delete:
            tableView.deleteRows(at: [indexPath!], with: .fade)
        case .update:
            configureCell(tableView.cellForRow(at: indexPath!)!, withName: anObject as! Contact)
        case .move:
            configureCell(tableView.cellForRow(at: indexPath!)!, withName: anObject as! Contact)
            tableView.moveRow(at: indexPath!, to: newIndexPath!)
        }
    }
    
    func controllerDidChangeContent(_ controller: NSFetchedResultsController<NSFetchRequestResult>) {
        tableView.endUpdates()
    }
    
    /*
     // Implementing the above methods to update the table view in response to individual changes may have performance implications if a large number of changes are made simultaneously. If this proves to be an issue, you can instead just implement controllerDidChangeContent: which notifies the delegate that all section and object changes have been processed.
     
     func controllerDidChangeContent(controller: NSFetchedResultsController) {
     // In the simplest, most efficient, case, reload the table view.
     tableView.reloadData()
     }
     */
    
    
    
    // MARK: - Table view data source
    
//    func fetch() {
//        guard let appDelegate = UIApplication.shared.delegate as? AppDelegate else { return }
//        let managedObjectContext = appDelegate.persistentContainer.viewContext
//        let fetchRequest = NSFetchRequest<NSFetchRequestResult>(entityName:"NameList")
//        do {
//            Names = try managedObjectContext.fetch(fetchRequest) as! [NSManagedObject]
//        } catch let error as NSError {
//            print("Could not fetch. \(error)")
//        }
//    }
//
//
    func save(contactName: String, contactDetails: String, contactType: Int, contactNumber: String, contactEmail: String, contactLocation: String, contactInfoDate: Date, contactInviteDate: Date, contactImage: UIImage) {
        let context = self.fetchedResultsController.managedObjectContext
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
    
    
    func update(indexPath: IndexPath, contactName: String, contactDetails: String, contactType: Int, contactNumber: String, contactEmail: String, contactLocation: String, contactInfoDate: Date, contactInviteDate: Date, contactImage: UIImage) {
  
        let context = self.fetchedResultsController.managedObjectContext
        fetchedResultsController.object(at: indexPath).contactName = contactName
        fetchedResultsController.object(at: indexPath).contactNumber = contactNumber
        fetchedResultsController.object(at: indexPath).contactEmail = contactEmail
        fetchedResultsController.object(at: indexPath).contactType = Int32(contactType)
        fetchedResultsController.object(at: indexPath).contactLocation = contactLocation
        fetchedResultsController.object(at: indexPath).contactInfoDate = contactInfoDate
        fetchedResultsController.object(at: indexPath).contactInviteDate = contactInviteDate
        fetchedResultsController.object(at: indexPath).contactDetails = contactDetails
        fetchedResultsController.object(at: indexPath).contactImage = UIImagePNGRepresentation(contactImage)
        
        do {

            try context.save()
        } catch {
            // Replace this implementation with code to handle the error appropriately.
            // fatalError() causes the application to generate a crash log and terminate. You should not use this function in a shipping application, although it may be useful during development.
            let nserror = error as NSError
            fatalError("Unresolved error \(nserror), \(nserror.userInfo)")
        }
    }
//
//    func delete(_ Name: NSManagedObject, at indexPath: IndexPath) {
//        guard let appDelegate = UIApplication.shared.delegate as? AppDelegate else { return }
//        let managedObjectContext = appDelegate.persistentContainer.viewContext
//        managedObjectContext.delete(Name)
//        Names.remove(at: indexPath.row)
//    }
    
    
    
    @IBAction func unwindToNameList(segue: UIStoryboardSegue) {
        if let viewController = segue.source as? AddNameViewController {
            guard let contactName: String = viewController.contactNameTextField.text,
                let contactNumber: String = viewController.contactNumberTextField.text,
                let contactEmail: String = viewController.contactEmailTextField.text,
                let contactType: Int = viewController.contactTypeSegmentedControl.selectedSegmentIndex,
                let contactLocation: String = viewController.contactLocationTextField.text,
                let contactInfoDate: Date = NSDate() as Date,
                let contactInviteDate: Date = NSDate() as Date,
                let contactDetails: String = viewController.contactDetailTextField.text,
                let contactImage: UIImage = viewController.contactImageView.image
                else { return }
            
//            let formatter = DateFormatter()
//            formatter.dateFormat = "dd/MM/yyyy"
//            txtDatePicker.text = formatter.string(from: datePicker.date)
            
            if contactName != "" && contactNumber != "" {
                if let indexPath = viewController.indexPathForName {
                    update(indexPath: indexPath, contactName:contactName, contactDetails: contactDetails, contactType: contactType, contactNumber: contactNumber, contactEmail: contactEmail, contactLocation: contactLocation, contactInfoDate: contactInfoDate, contactInviteDate: contactInviteDate, contactImage: contactImage)
                } else {
                    save(contactName:contactName, contactDetails: contactDetails, contactType: contactType, contactNumber: contactNumber, contactEmail: contactEmail, contactLocation: contactLocation, contactInfoDate: contactInfoDate, contactInviteDate: contactInviteDate, contactImage: contactImage)
                }
            }
            tableView.reloadData()
        } else if let viewController = segue.source as? NameListDetailViewController {
//            if viewController.isDeleted {
//                guard let indexPath: IndexPath = viewController.indexPath else { return }
//                let Name = Names[indexPath.row]
//                delete(Name, at: indexPath)
                tableView.reloadData()
//            }
        }
    }
}

