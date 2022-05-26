//
//  MasterViewController.swift
//  D HansSs Business Manager
//
//  Created by Mrunal Kanta Muduli on 29/09/17.
//  Copyright © 2017 Mrunal Kanta Muduli. All rights reserved.
//

import UIKit
import CoreData

class DreamListViewController: UITableViewController, NSFetchedResultsControllerDelegate, UISearchResultsUpdating {
    var detailViewController: DreamListDetailViewController? = nil
    var managedObjectContext: NSManagedObjectContext? = nil
    
    let searchController = UISearchController(searchResultsController: nil)
    var searchPredicate: NSPredicate? = nil
    var filteredObjects : [Dream]? = nil
    
    func filterContentForSearchText(searchText: String, scope: String = "All" )
    {
        searchPredicate = NSPredicate(format: "dreamName contains[c] %@", searchText)
        filteredObjects = self.fetchedResultsController.fetchedObjects?.filter() {
            return self.searchPredicate!.evaluate(with: $0)
            } as [Dream]?
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
            detailViewController = (controllers[controllers.count-1] as! UINavigationController).topViewController as? DreamListDetailViewController
        }
        
        // UISearchController setup
        searchController.dimsBackgroundDuringPresentation = false
        searchController.searchResultsUpdater = self
//        searchController.searchBar.sizeToFit()
        self.tableView.tableHeaderView = searchController.searchBar
        self.definesPresentationContext = true
//        self.tableView.delegate = self
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
        if segue.identifier == "showDreamDetail" {
            if let indexPath = tableView.indexPathForSelectedRow {
                var object : Dream? = nil
                if searchController.isActive && searchController.searchBar.text != ""
                {
                    object = filteredObjects?[indexPath.row]
                }
                else{
                    object = fetchedResultsController.object(at: indexPath)
                }
                let controller = (segue.destination as! UINavigationController).topViewController as! DreamListDetailViewController
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
        let cell = tableView.dequeueReusableCell(withIdentifier: "DreamCell", for: indexPath)
        
        var dream : Dream? = nil
        if searchController.isActive && searchController.searchBar.text != ""{
            dream = filteredObjects?[indexPath.row]
        }else{
            dream = fetchedResultsController.object(at: indexPath)
        }
        configureCell(cell, withDream: dream!)
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
    
    func configureCell(_ cell: UITableViewCell, withDream dream: Dream) {
        
        cell.textLabel!.text = dream.dreamName!.description
        cell.detailTextLabel?.text = dream.dreamDesc!.description
        cell.imageView?.image = UIImage(data: dream.dreamImage!)
        //TODO - save image saving of default type again and again
    }
    
    // MARK: - Fetched results controller
    
    var fetchedResultsController: NSFetchedResultsController<Dream> {
        if _fetchedResultsController != nil {
            return _fetchedResultsController!
        }
        
        let fetchRequest: NSFetchRequest<Dream> = Dream.fetchRequest()
        
        // Set the batch size to a suitable number.
        fetchRequest.fetchBatchSize = 20
        
        // Edit the sort key as appropriate.
        let sortDescriptor = NSSortDescriptor(key: "dreamName", ascending: false)
        
        fetchRequest.sortDescriptors = [sortDescriptor]
        
        // Edit the section name key path and cache name if appropriate.
        // nil for section name key path means "no sections".
        let aFetchedResultsController = NSFetchedResultsController(fetchRequest: fetchRequest, managedObjectContext: self.managedObjectContext!, sectionNameKeyPath: nil, cacheName: "DreamList")
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
    var _fetchedResultsController: NSFetchedResultsController<Dream>? = nil
    
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
            configureCell(tableView.cellForRow(at: indexPath!)!, withDream: anObject as! Dream)
        case .move:
            configureCell(tableView.cellForRow(at: indexPath!)!, withDream: anObject as! Dream)
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
//        let fetchRequest = NSFetchRequest<NSFetchRequestResult>(entityName:"DreamList")
//        do {
//            dreams = try managedObjectContext.fetch(fetchRequest) as! [NSManagedObject]
//        } catch let error as NSError {
//            print("Could not fetch. \(error)")
//        }
//    }
//
//
    func save(dreamName: String, dreamDesc: String, dreamType: Int, dreamDate: Date,dreamImage: UIImage) {
        let context = self.fetchedResultsController.managedObjectContext
        let newDream = Dream(context: context)
        // If appropriate, configure the new managed object.
        newDream.dreamName = dreamName //String("iPhone")
        newDream.dreamDesc = dreamDesc //ßString("iPhone X")
        newDream.dreamDate = dreamDate //NSDate() as Date
        newDream.dreamAchieved = false
        newDream.dreamType = Int32(dreamType)
//        let img = UIImage(named: "DefaultImage")
        let imageData = UIImagePNGRepresentation(dreamImage)
        newDream.dreamImage = imageData
        
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
    
    
    func update(indexPath: IndexPath, dreamName: String, dreamDesc: String, dreamType: Int, dreamDate: Date, dreamAchieved: Bool, dreamImage: UIImage){
  
        let context = self.fetchedResultsController.managedObjectContext
        fetchedResultsController.object(at: indexPath).dreamName = dreamName
        fetchedResultsController.object(at: indexPath).dreamDesc = dreamDesc
        fetchedResultsController.object(at: indexPath).dreamDate = dreamDate
        fetchedResultsController.object(at: indexPath).dreamType = Int32(dreamType)
        fetchedResultsController.object(at: indexPath).dreamAchieved = dreamAchieved
        fetchedResultsController.object(at: indexPath).dreamImage = UIImagePNGRepresentation(dreamImage)
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
//    func delete(_ dream: NSManagedObject, at indexPath: IndexPath) {
//        guard let appDelegate = UIApplication.shared.delegate as? AppDelegate else { return }
//        let managedObjectContext = appDelegate.persistentContainer.viewContext
//        managedObjectContext.delete(dream)
//        dreams.remove(at: indexPath.row)
//    }
    
    
    
    @IBAction func unwindToDreamList(segue: UIStoryboardSegue) {
        if let viewController = segue.source as? AddDreamViewController {
            guard let dreamName: String = viewController.dreamNameTextField.text,
                let dreamDesc: String = viewController.dreamDescTextField.text,
                let dreamType: Int = viewController.dreamTypeSegmentedControl.selectedSegmentIndex,
                let dreamDate: Date = viewController.dreamDatePicker.date,
                let dreamAchieved: Bool = false,
                let dreamImage: UIImage = viewController.dreamImageView.image
            else { return }
            
//            let formatter = DateFormatter()
//            formatter.dateFormat = "dd/MM/yyyy"
//            txtDatePicker.text = formatter.string(from: datePicker.date)
            
            if dreamName != "" && dreamDesc != "" {
                if let indexPath = viewController.indexPathForDream {
                    update(indexPath: indexPath, dreamName:dreamName, dreamDesc:dreamDesc, dreamType:dreamType, dreamDate:dreamDate, dreamAchieved:dreamAchieved, dreamImage:dreamImage)
                } else {
                    save(dreamName:dreamName, dreamDesc:dreamDesc, dreamType:dreamType, dreamDate:dreamDate,dreamImage:dreamImage)
                }
            }
            tableView.reloadData()
        } else if let viewController = segue.source as? DreamListDetailViewController {
//            if viewController.isDeleted {
//                guard let indexPath: IndexPath = viewController.indexPath else { return }
//                let dream = dreams[indexPath.row]
//                delete(dream, at: indexPath)
                tableView.reloadData()
//            }
        }
    }
}

