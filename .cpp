#include <iostream>
void n_1() {
    int a=7;
    int b=2;
    int delenie_1 = a / b;
    double delenie = static_cast<double>(a) / b;
    

    std::cout << "celoe: " << delenie_1 << std::endl;
    std::cout << "drobnoe: " << delenie << std::endl;
}
void n_2() {
    double a = 7;
    
    std::cout << "Enter integer number: ";
    std::cin >> a;

    std::cout << "a = ";
    std::cin >> a;
}
    void n_3() {
     const double GRAVITY = 9.8;

    }
    void n_4() {
    int grade; 
    
    std::cout << "Bawa ocenka (Celoe 4uclo ot 1 do 5): ";
    std::cin >> grade;
    
    char gradeChar = '0' + grade; 

   
    std::cout << "Bawa ocenka: " << gradeChar << std::endl;
    
    }
    void n_7() {
   char symbol = 'A';
   int index = symbol;
   std::cout << "index A in ASCII: " << index << std::endl;
    }
    void n_5() {
        int num1, num2;

    std::cout << "Bbedute IIepBoe Celoe 4uclo: ";
    std::cin >> num1;
    
    std::cout << "Bbedute Btopoe Celoe 4uclo: ";
    std::cin >> num2;

    double average = static_cast<double>(num1 + num2) / 2;

    std::cout << "Cpednee Apefmiti4eckoe: " << average << std::endl;

    }
void n_6(){
    const int MAX_SCORE = 1000;
    std::cout << "Max Ocenka: " << MAX_SCORE << std::endl;
    //MAX_SCORE = 2000;
    std::cout << "IIoIIbITka U3menenuya MAX_SCORE na 2000." << std::endl;
}
void n_8(){
    double num;
    std::cout << "Bbedute dpobnoe 4uclo: ";
    std::cin >> num;
     int intNum = static_cast<int>(num);
      std::cout << "Celaya 4acTb 4ucla: " << intNum << std::endl;

}
void n_9(){
    const int numerator = 100;
    const int denominator = 3;

    int intResult = numerator / denominator;
    std::cout << "Pe3ylbTat Delenuya 100 na 3 kak int: " << intResult << std::endl;
    double doubleResult = static_cast<double>(numerator) / denominator;
    std::cout << "Pe3ylbTat Delenuya 100 na 3 kak double: " << doubleResult << std::endl;
}
void n_10(){
    int num;
     std::cout << "Bbedute dpobnoe 4uclo: ";
     double inputNum;
     std::cin >> inputNum;
     num = static_cast<int>(inputNum);
     std::cout << "Celaya 4acTb 4ucla: " << num << std::endl;

}
int main() {
    n_1();
    n_2();
    n_3();
    n_4();
    n_7();
    n_5();
    n_6();
    n_8();
    n_9();
    n_10();
    return 0;
}
