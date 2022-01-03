//
//  ViewController.swift
//  The Gym Hero
//
//  Created by Hoonsbrand on 12/14/21.
//

import UIKit

extension String {
    var localized: String {
        return NSLocalizedString(self, tableName: nil, bundle: Bundle.main, value: "", comment: "")
    }
}

class ViewController: UIViewController {

    @IBOutlet weak var selectModeLabel: UILabel!
    @IBOutlet weak var timerModeLabel: UIButton!
    @IBOutlet weak var countingModeLabel: UIButton!
    
    override func viewDidLoad() {
        super.viewDidLoad()
        localLang()
    }
    
    // select language in this app
//    @objc func gearTapped() {
//        let alert = UIAlertController(title: "Language Setting".localized, message: "Choose the language.".localized, preferredStyle: .actionSheet)
//
//        let lang_Kor = UIAlertAction(title: "Korean".localized, style: .default) { (action) in
//            UserDefaults.standard.set(["ko"], forKey: "AppleLanguages")
//            UserDefaults.standard.synchronize()
//
//            self.setLanguage()
//
//        }
        
//        let lang_Eng = UIAlertAction(title: "English".localized, style: .default) { (action) in
//            UserDefaults.standard.set(["en"], forKey: "AppleLanguages")
//            UserDefaults.standard.synchronize()
//
//            self.setLanguage()
//        }
//
//        let cancel = UIAlertAction(title: "cancel".localized, style: .cancel) {
//            (cancel) in
//        }
//        alert.addAction(lang_Kor)
//        alert.addAction(lang_Eng)
//        alert.addAction(cancel)
//
//        self.present(alert, animated: true, completion: nil)
//    }
//
//    @objc func profileTapped(sender:UIBarButtonItem) {
//        performSegue(withIdentifier: "ProfileEditPage", sender: nil)
//    }
//
    @IBAction func TimerMode(_ sender: UIButton) {
        let vc = storyboard?.instantiateViewController(withIdentifier: "TimerMode") as! TimerModeViewController
        vc.title = "Timer Mode"
        navigationController?.pushViewController(vc, animated: true)
    }
    
    @IBAction func CountingMode(_ sender: UIButton) {
        let vc = storyboard?.instantiateViewController(withIdentifier: "CountingMode") as! CountingModeViewController
        vc.title = "Counting Mode"
        navigationController?.pushViewController(vc, animated: true)
    }
    
    func localLang() {
        selectModeLabel.text = "Select The Mode!".localized
        timerModeLabel.setTitle("Timer Mode".localized, for: .normal)
        countingModeLabel.setTitle("Counting Mode".localized, for: .normal)
    }
    
//    func setLanguage() {
//        
//        // 설정된 언어 코드 가져오기
//        let language = UserDefaults.standard.array(forKey: "AppleLanguages")?.first as! String
//        let index = language.index(language.startIndex, offsetBy: 2)
//        let languageCode = String(language[..<index]) //"ko" , "en" 등
//        
//        //설정된 언어 파일 가져오기
//        let path = Bundle.main.path(forResource: languageCode, ofType: "lproj")
//        let bundle = Bundle(path: path!)
//        
//        selectModeLabel.text = bundle?.localizedString(forKey: "Select The Mode!", value: nil, table: nil)
//        timerModeLabel.setTitle(bundle?.localizedString(forKey: "Timer Mode", value: nil, table: nil), for: .normal)
//        countingModeLabel.setTitle(bundle?.localizedString(forKey: "Counting Mode", value: nil, table: nil), for: .normal)
//        bundle?.localizedString(forKey: "Language Setting", value: nil, table: nil)
//        bundle?.localizedString(forKey: "Choose the language", value: nil, table: nil)
//        bundle?.localizedString(forKey: "Korean", value: nil, table: nil)
//        bundle?.localizedString(forKey: "English", value: nil, table: nil)
//    }

}
