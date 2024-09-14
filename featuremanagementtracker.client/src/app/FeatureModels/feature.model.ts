export interface Feature {
  id?: number;
  title: string;
  description: string;
  complexity: 'S' | 'M' | 'L' | 'XL';
  status: 'New' | 'Active' | 'Closed' | 'Abandoned';
  targetDate?: Date;
  actualDate?: Date;
}
