import { useState } from 'react';
import { Questions, AddQuestion } from '../../components/Forum';

const ForumPage = () => {
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [refreshTrigger, setRefreshTrigger] = useState(0);

  const handleQuestionAdded = () => {
    setRefreshTrigger(prev => prev + 1); // Для обновления списка вопросов
  };

  return (
    <div className="forum-page">
      <Questions 
        key={refreshTrigger} // Принудительное обновление при изменении
        onNewQuestionClick={() => setIsModalOpen(true)}
      />
      
      <AddQuestion
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        onQuestionAdded={handleQuestionAdded}
      />
    </div>
  );
};

export default ForumPage;