//
//  DetailViewController.swift
//  D HansSs Business Manager
//
//  Created by Mrunal Kanta Muduli on 29/09/17.
//  Copyright © 2017 Mrunal Kanta Muduli. All rights reserved.
//

import UIKit

class DreamListDetailViewController: UIViewController {

    @IBOutlet weak var detailDescriptionLabel: UILabel!
    @IBOutlet weak var dreamDescLabel: UILabel!
    @IBOutlet weak var dreamDateLabel: UILabel!
    @IBOutlet weak var dreamIsAchievedLabel: UILabel!
    @IBOutlet weak var dreamTypeLabel: UILabel!
    @IBOutlet weak var dreamImage: UIImageView!
    
    var indexPath: IndexPath? = nil

    func configureView() {
        let dateFormatter = DateFormatter()
        dateFormatter.dateFormat = "d MMMM yyyy, h:mm a"
        // Update the user interface for the detail item.
        if let detail = detailItem {
            if let label = detailDescriptionLabel {
                label.text = detail.dreamName!
            }
            if let label = dreamDescLabel {
                label.text = detail.dreamDesc!.description
            }
            if let label = dreamDateLabel {
                let dreamDate = dateFormatter.string(from: detail.dreamDate!)
                label.text = dreamDate.description
            }
            if let label = dreamIsAchievedLabel {
                label.text = detail.dreamAchieved.description
            }
            if let label = dreamTypeLabel {
                label.text = detail.dreamType.description
            }
            if let image = dreamImage {
                image.image = UIImage(data: detail.dreamImage!)
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

    var detailItem: Dream? {
        didSet {
            // Update the view.
            configureView()
        }
    }

    override func prepare(for segue: UIStoryboardSegue, sender: Any?) {
        if segue.identifier == "editDream" {
            guard let viewController = segue.destination as? AddDreamViewController else { return }
            viewController.titleText = "Edit Dream"
            viewController.dream = self.detailItem
            viewController.indexPathForDream = self.indexPath!
        }
    }

}

